using System;
using System.Net.Sockets;
using System.Threading;
using C5;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Events;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public class NetworkGame
	{
		#region Constants

		private const int ReceiveTimeout = 1000; // milliseconds
		private const int ProtocolVersion = 2;

		private const int RMSG_NEWWDG = 0;
		private const int RMSG_WDGMSG = 1;
		private const int RMSG_DSTWDG = 2;
		private const int RMSG_MAPIV = 3;
		private const int RMSG_GLOBLOB = 4;
		private const int RMSG_PAGINAE = 5;
		private const int RMSG_RESID = 6;
		private const int RMSG_PARTY = 7;
		private const int RMSG_SFX = 8;
		private const int RMSG_CATTR = 9;
		private const int RMSG_MUSIC = 10;
		private const int RMSG_TILES = 11;
		private const int RMSG_BUFF = 12;

		private const int GMSG_TIME = 0;
		private const int GMSG_ASTRO = 1;
		private const int GMSG_LIGHT = 2;

		private const int OD_REM = 0;
		private const int OD_MOVE = 1;
		private const int OD_RES = 2;
		private const int OD_LINBEG = 3;
		private const int OD_LINSTEP = 4;
		private const int OD_SPEECH = 5;
		private const int OD_LAYERS = 6;
		private const int OD_DRAWOFF = 7;
		private const int OD_LUMIN = 8;
		private const int OD_AVATAR = 9;
		private const int OD_FOLLOW = 10;
		private const int OD_HOMING = 11;
		private const int OD_OVERLAY = 12;
		/* private const int OD_AUTH = 13; -- Removed */
		private const int OD_HEALTH = 14;
		private const int OD_BUDDY = 15;
		private const int OD_END = 255;

		private const int PD_LIST = 0;
		private const int PD_LEADER = 1;
		private const int PD_MEMBER = 2;

		#endregion

		private static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

		private readonly MessageSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private readonly object stateLock = new object();
		private readonly TreeDictionary<ushort, Message> waiting;
		private readonly TreeDictionary<int, FragmentBuffer> mapFrags;
		private readonly CompositeGameEventListener listener;
		private ushort rseq;
		private NetworkGameState state;

		public NetworkGame(NetworkAddress address)
		{
			waiting = new TreeDictionary<ushort, Message>();
			mapFrags = new TreeDictionary<int, FragmentBuffer>();
			listener = new CompositeGameEventListener();

			state = NetworkGameState.Created;
			socket = new MessageSocket(address.Host, address.Port);
			socket.SetReceiveTimeout(ReceiveTimeout);
			sender = new MessageSender(socket);
			sender.Finished += OnTaskFinished;
			receiver = new MessageReceiver(socket, ReceiveMessage);
			receiver.Finished += OnTaskFinished;
		}

		public event Action Stopped;

		public void Start(string userName, byte[] cookie)
		{
			try
			{
				lock (stateLock)
				{
					if (state != NetworkGameState.Created)
						throw new InvalidOperationException("Can't open already opened/closed connection");

					Connect(userName, cookie);
					receiver.Run();
					sender.Run();

					state = NetworkGameState.Started;
				}
			}
			catch (SocketException ex)
			{
				throw new NetworkException(ex.Message, ex);
			}
		}

		public void Stop()
		{
			lock (stateLock)
			{
				if (state != NetworkGameState.Started)
					return;

				receiver.Finished -= OnTaskFinished;
				receiver.Stop();

				sender.Finished -= OnTaskFinished;
				sender.Stop();

				socket.Send(new Message(Message.MSG_CLOSE));
				// HACK: otherwise close message may not be sent
				Thread.Sleep(TimeSpan.FromMilliseconds(5));
				socket.Close();

				state = NetworkGameState.Stopped;
			}
			Stopped.Raise();
		}

		public void AddListener(IGameEventListener l)
		{
			listener.AddListener(l);
		}

		public void RemoveListener(IGameEventListener l)
		{
			listener.RemoveListener(l);
		}

		public void RequestMap(int x, int y)
		{
			var msg = new Message(Message.MSG_MAPREQ).Coord(x, y);
			socket.Send(msg);
		}

		public void MessageWidget(ushort widgetId, string name, object[] args)
		{
			var message = new Message(RMSG_WDGMSG)
				.Uint16(widgetId)
				.String(name);
			if (args != null)
				message.List(args);
			sender.SendMessage(message);
		}

		private void SendObjectAck(int id, int frame)
		{
			// FIXME: make it smarter
			socket.Send(new Message(Message.MSG_OBJACK).Int32(id).Int32(frame));
		}

		private void Connect(string userName, byte[] cookie)
		{
			socket.Connect();

			var hello = new Message(Message.MSG_SESS)
				.Uint16(1)
				.String("Haven")
				.Uint16(ProtocolVersion)
				.String(userName)
				.Bytes(cookie);

			socket.Send(hello);

			Message reply;
			ConnectionError error;
			while (true)
			{
				if (!socket.Receive(out reply))
					throw new NetworkException(ConnectionError.ConnectionError);

				if (reply.Type == Message.MSG_SESS)
				{
					error = (ConnectionError)reply.Buffer.ReadByte();
					break;
				}
				if (reply.Type == Message.MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new NetworkException(error);
		}

		private void ReceiveMessage(Message msg)
		{
			var buffer = msg.Buffer;
			switch (msg.Type)
			{
				case Message.MSG_REL:
					var seq = buffer.ReadUInt16();
					while (buffer.HasRemaining)
					{
						var type = buffer.ReadByte();
						int len;
						if ((type & 0x80) != 0) // is not last?
						{
							type &= 0x7f;
							len = buffer.ReadUInt16();
						}
						else
							len = (int)buffer.Remaining;
						GotRel(seq, new Message(type, buffer.ReadBytes(len)));
						seq++;
					}
					break;
				case Message.MSG_ACK:
					sender.ReceiveAck(buffer.ReadUInt16());
					break;
				case Message.MSG_MAPDATA:
					HandleMapData(buffer);
					break;
				case Message.MSG_OBJDATA:
					while (buffer.HasRemaining)
						HandleGobData(buffer);
					break;
				case Message.MSG_CLOSE:
					Log.Info("Server dropped connection");
					Stop();
					return;
			}
		}

		private void GotRel(ushort seq, Message msg)
		{
			if (seq == rseq)
			{
				HandleRel(msg);
				while (true)
				{
					rseq++;
					if (!waiting.Remove(rseq, out msg))
						break;
					HandleRel(msg);
				}
				sender.SendAck((ushort)(rseq - 1));
			}
			else if (seq > rseq)
				waiting[seq] = msg;
		}

		private void HandleRel(Message msg)
		{
			var buf = msg.Buffer;
			switch (msg.Type)
			{
				case RMSG_NEWWDG:
					listener.Handle(buf.ReadWidgetCreateEvent());
					break;
				case RMSG_WDGMSG:
					listener.Handle(buf.ReadWidgetMessageEvent());
					break;
				case RMSG_DSTWDG:
					listener.Handle(buf.ReadWidgetDestroyEvent());
					break;
				case RMSG_MAPIV:
				{
					int type = buf.ReadByte();
					switch (type)
					{
						case 0:
							listener.Handle(buf.ReadMapInvalidateGridEvent());
							break;
						case 1:
							listener.Handle(buf.ReadMapInvalidateRegionEvent());
							break;
						case 2:
							listener.Handle(new MapInvalidateEvent());
							break;
					}
					break;
				}
				case RMSG_GLOBLOB:
					while (buf.HasRemaining)
					{
						switch (buf.ReadByte())
						{
							case GMSG_TIME:
								listener.Handle(buf.ReadGameTimeUpdateEvent());
								break;
							case GMSG_ASTRO:
								listener.Handle(buf.ReadAstronomyUpdateEvent());
								break;
							case GMSG_LIGHT:
								listener.Handle(buf.ReadAmbientLightUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_PAGINAE:
					listener.Handle(buf.ReadGameActionsUpdateEvent());
					break;
				case RMSG_RESID:
					listener.Handle(buf.ReadResourceLoadEvent());
					break;
				case RMSG_PARTY:
					while (buf.HasRemaining)
					{
						int type = buf.ReadByte();
						switch (type)
						{
							case PD_LIST:
								listener.Handle(buf.ReadPartyUpdateEvent());
								break;
							case PD_LEADER:
								listener.Handle(buf.ReadPartyLeaderChangeEvent());
								break;
							case PD_MEMBER:
								listener.Handle(buf.ReadPartyMemberUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_SFX:
					listener.Handle(buf.ReadPlaySoundEvent());
					break;
				case RMSG_CATTR:
					listener.Handle(buf.ReadCharAttributesUpdateEvent());
					break;
				case RMSG_MUSIC:
					listener.Handle(new PlayMusicEvent());
					break;
				case RMSG_TILES:
					listener.Handle(buf.ReadTilesetsLoadEvent());
					break;
				case RMSG_BUFF:
				{
					var message = buf.ReadCString();
					switch (message)
					{
						case "clear":
							listener.Handle(new BuffClearEvent());
							break;
						case "rm":
							listener.Handle(buf.ReadBuffRemoveEvent());
							break;
						case "set":
							listener.Handle(buf.ReadBuffUpdateEvent());
							break;
					}
					break;
				}
				default:
					throw new Exception("Unknown rmsg type: " + msg.Type);
			}
		}

		// TODO: delete lingering fragments?
		private void HandleMapData(ByteBuffer reader)
		{
			int packetId = reader.ReadInt32();
			int offset = reader.ReadUInt16();
			int length = reader.ReadUInt16();

			FragmentBuffer fragbuf;
			if (mapFrags.Find(ref packetId, out fragbuf))
			{
				fragbuf.Add(offset, reader.ReadRemaining());
				if (fragbuf.IsComplete)
				{
					mapFrags.Remove(packetId);
					var fragReader = new ByteBuffer(fragbuf.Content);
					listener.Handle(fragReader.ReadMapUpdateEvent());
				}
			}
			else if (offset != 0 || reader.Length - 8 < length)
			{
				fragbuf = new FragmentBuffer(length);
				fragbuf.Add(offset, reader.ReadRemaining());
				mapFrags[packetId] = fragbuf;
			}
			else
			{
				listener.Handle(reader.ReadMapUpdateEvent());
			}
		}

		private void HandleGobData(ByteBuffer reader)
		{
			var ev = new GobUpdateEvent();
			ev.ReplaceFlag = (reader.ReadByte() & 1) != 0;
			ev.GobId = reader.ReadInt32();
			ev.Frame = reader.ReadInt32();
			while (true)
			{
				var delta = ReadGobDelta(reader);
				if (delta == null)
					break;
				ev.Deltas.Add(delta);
			}
			listener.Handle(ev);
			SendObjectAck(ev.GobId, ev.Frame);
		}

		private GobDelta ReadGobDelta(ByteBuffer reader)
		{
			int type = reader.ReadByte();
			switch (type)
			{
				case OD_REM:
					return new GobDelta.Clear();
				case OD_MOVE:
					return reader.ReadGobPosition();
				case OD_RES:
					return reader.ReadGobResource();
				case OD_LINBEG:
					return reader.ReadGobStartMovement();
				case OD_LINSTEP:
					return reader.ReadGobAdjustMovement();
				case OD_SPEECH:
					return reader.ReadGobSpeech();
				case OD_LAYERS:
					return reader.ReadGobLayers();
				case OD_AVATAR:
					return reader.ReadGobAvatar();
				case OD_DRAWOFF:
					return reader.ReadGobDrawOffset();
				case OD_LUMIN:
					return reader.ReadGobLight();
				case OD_FOLLOW:
					return reader.ReadGobFollow();
				case OD_HOMING:
					return reader.ReadGobHoming();
				case OD_OVERLAY:
					return reader.ReadGobOverlay();
				case OD_HEALTH:
					return reader.ReadGobHealth();
				case OD_BUDDY:
					return reader.ReadGobBuddy();
				case OD_END:
					return null;
				default:
					// TODO: MessageException
					throw new Exception("Unknown objdelta type: " + type);
			}
		}

		private void OnTaskFinished(object sender, EventArgs args)
		{
			// it shouldn't happen normally, so let it crash
			throw new Exception("Task finished abruptly");
		}
	}
}
