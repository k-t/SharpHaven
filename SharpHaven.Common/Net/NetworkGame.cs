using System;
using System.Net.Sockets;
using System.Threading;
using C5;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Messages;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public class NetworkGame
	{
		#region Constants

		private const int ReceiveTimeout = 1000; // milliseconds
		private const int ProtocolVersion = 2;

		public const int MSG_SESS = 0;
		public const int MSG_REL = 1;
		public const int MSG_ACK = 2;
		public const int MSG_BEAT = 3;
		public const int MSG_MAPREQ = 4;
		public const int MSG_MAPDATA = 5;
		public const int MSG_OBJDATA = 6;
		public const int MSG_OBJACK = 7;
		public const int MSG_CLOSE = 8;

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

		private readonly BinaryMessageSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private readonly object stateLock = new object();
		private readonly TreeDictionary<ushort, BinaryMessage> waiting;
		private readonly TreeDictionary<int, FragmentBuffer> mapFrags;
		private readonly IMessagePublisher publisher;
		private ushort rseq;
		private NetworkGameState state;

		public NetworkGame(NetworkAddress address, IMessagePublisher publisher)
		{
			this.publisher = publisher;
			this.waiting = new TreeDictionary<ushort, BinaryMessage>();
			this.mapFrags = new TreeDictionary<int, FragmentBuffer>();

			this.state = NetworkGameState.Created;
			this.socket = new BinaryMessageSocket(address.Host, address.Port);
			this.socket.SetReceiveTimeout(ReceiveTimeout);
			this.sender = new MessageSender(socket);
			this.sender.Finished += OnTaskFinished;
			this.receiver = new MessageReceiver(socket, ReceiveMessage);
			this.receiver.Finished += OnTaskFinished;
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

				socket.Send(BinaryMessage.Make(MSG_CLOSE).Complete());
				// HACK: otherwise close message may not be sent
				Thread.Sleep(TimeSpan.FromMilliseconds(5));
				socket.Close();

				state = NetworkGameState.Stopped;
			}
			Stopped.Raise();
		}

		public void Send<TMessage>(TMessage message)
		{
			var gridRequest = message as MapRequestGrid;
			if (gridRequest != null)
			{
				var msg = BinaryMessage.Make(MSG_MAPREQ)
					.Coord(gridRequest.Coord)
					.Complete();
				socket.Send(msg);
				return;
			}

			var widgetMessage = message as WidgetMessage;
			if (widgetMessage != null)
			{
				var msg = BinaryMessage.Make(RMSG_WDGMSG)
					.UInt16(widgetMessage.WidgetId)
					.String(widgetMessage.Name);

				if (widgetMessage.Args != null)
					msg.List(widgetMessage.Args);

				sender.SendMessage(msg.Complete());
				return;
			}

			throw new ArgumentException($"Unsupported outgoing messages type '{message.GetType().Name}'");
		}

		private void SendObjectAck(int id, int frame)
		{
			var message = BinaryMessage.Make(MSG_OBJACK)
				.Int32(id)
				.Int32(frame)
				.Complete();
			// FIXME: make it smarter
			socket.Send(message);
		}

		private void Connect(string userName, byte[] cookie)
		{
			socket.Connect();

			var hello = BinaryMessage.Make(MSG_SESS)
				.UInt16(1)
				.String("Haven")
				.UInt16(ProtocolVersion)
				.String(userName)
				.Bytes(cookie)
				.Complete();

			socket.Send(hello);

			BinaryMessage reply;
			ConnectionError error;
			while (true)
			{
				if (!socket.Receive(out reply))
					throw new NetworkException(ConnectionError.ConnectionError);

				if (reply.Type == MSG_SESS)
				{
					error = (ConnectionError)reply.GetReader().ReadByte();
					break;
				}
				if (reply.Type == MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new NetworkException(error);
		}

		private void ReceiveMessage(BinaryMessage msg)
		{
			var buffer = msg.GetReader();
			switch (msg.Type)
			{
				case MSG_REL:
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
						GotRel(seq, new BinaryMessage(type, buffer.ReadBytes(len)));
						seq++;
					}
					break;
				case MSG_ACK:
					sender.ReceiveAck(buffer.ReadUInt16());
					break;
				case MSG_MAPDATA:
					HandleMapData(buffer);
					break;
				case MSG_OBJDATA:
					while (buffer.HasRemaining)
						HandleGobData(buffer);
					break;
				case MSG_CLOSE:
					Log.Info("Server dropped connection");
					Stop();
					return;
			}
		}

		private void GotRel(ushort seq, BinaryMessage msg)
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

		private void HandleRel(BinaryMessage msg)
		{
			var buf = msg.GetReader();
			switch (msg.Type)
			{
				case RMSG_NEWWDG:
					publisher.Publish(buf.ReadWidgetCreateEvent());
					break;
				case RMSG_WDGMSG:
					publisher.Publish(buf.ReadWidgetMessageEvent());
					break;
				case RMSG_DSTWDG:
					publisher.Publish(buf.ReadWidgetDestroyEvent());
					break;
				case RMSG_MAPIV:
				{
					int type = buf.ReadByte();
					switch (type)
					{
						case 0:
							publisher.Publish(buf.ReadMapInvalidateGridEvent());
							break;
						case 1:
							publisher.Publish(buf.ReadMapInvalidateRegionEvent());
							break;
						case 2:
							publisher.Publish(new MapInvalidate());
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
								publisher.Publish(buf.ReadGameTimeUpdateEvent());
								break;
							case GMSG_ASTRO:
								publisher.Publish(buf.ReadAstronomyUpdateEvent());
								break;
							case GMSG_LIGHT:
								publisher.Publish(buf.ReadAmbientLightUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_PAGINAE:
					publisher.Publish(buf.ReadGameActionsUpdateEvent());
					break;
				case RMSG_RESID:
					publisher.Publish(buf.ReadResourceLoadEvent());
					break;
				case RMSG_PARTY:
					while (buf.HasRemaining)
					{
						int type = buf.ReadByte();
						switch (type)
						{
							case PD_LIST:
								publisher.Publish(buf.ReadPartyUpdateEvent());
								break;
							case PD_LEADER:
								publisher.Publish(buf.ReadPartyLeaderChangeEvent());
								break;
							case PD_MEMBER:
								publisher.Publish(buf.ReadPartyMemberUpdateEvent());
								break;
						}
					}
					break;
				case RMSG_SFX:
					publisher.Publish(buf.ReadPlaySoundEvent());
					break;
				case RMSG_CATTR:
					publisher.Publish(buf.ReadCharAttributesUpdateEvent());
					break;
				case RMSG_MUSIC:
					publisher.Publish(new PlayMusic());
					break;
				case RMSG_TILES:
					publisher.Publish(buf.ReadTilesetsLoadEvent());
					break;
				case RMSG_BUFF:
				{
					var message = buf.ReadCString();
					switch (message)
					{
						case "clear":
							publisher.Publish(new BuffClearAll());
							break;
						case "rm":
							publisher.Publish(buf.ReadBuffRemoveEvent());
							break;
						case "set":
							publisher.Publish(buf.ReadBuffUpdateEvent());
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
					publisher.Publish(fragReader.ReadMapUpdateEvent());
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
				publisher.Publish(reader.ReadMapUpdateEvent());
			}
		}

		private void HandleGobData(ByteBuffer reader)
		{
			var ev = new UpdateGameObject();
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
			publisher.Publish(ev);
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
