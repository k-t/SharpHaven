using System;
using System.Collections.Generic;
using System.Net.Sockets;
using C5;
using MonoHaven.Network.Messages;
using NLog;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
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

		#endregion

		private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

		private readonly ConnectionSettings settings;
		private readonly GameSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private ConnectionState state;
		private readonly object stateLock = new object();
		private ushort rseq;
		private readonly TreeDictionary<ushort, MessageReader> waiting;
		private readonly TreeDictionary<int, FragmentBuffer> mapFrags;
		private readonly List<IConnectionListener> listeners;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			waiting = new TreeDictionary<ushort, MessageReader>();
			mapFrags = new TreeDictionary<int, FragmentBuffer>();
			listeners = new List<IConnectionListener>();

			state = ConnectionState.Created;
			socket = new GameSocket(settings.Host, settings.Port);
			socket.SetReceiveTimeout(ReceiveTimeout);
			sender = new MessageSender(socket);
			sender.Finished += OnTaskFinished;
			receiver = new MessageReceiver(socket, ReceiveMessage);
			receiver.Finished += OnTaskFinished;
		}

		public event Action Closed;

		public void Dispose()
		{
			Close();
		}

		public void AddListener(IConnectionListener listener)
		{
			listeners.Add(listener);
		}

		public void RemoveListener(IConnectionListener listener)
		{
			listeners.Remove(listener);
		}

		public void Open()
		{
			try
			{
				lock (stateLock)
				{
					if (state != ConnectionState.Created)
						throw new InvalidOperationException("Can't open already opened/closed connection");

					Connect();
					receiver.Run();
					sender.Run();

					state = ConnectionState.Opened;
				}
			}
			catch (SocketException ex)
			{
				throw new ConnectionException(ex.Message, ex);
			}
		}

		public void Close()
		{
			lock (stateLock)
			{
				if (state != ConnectionState.Opened)
					return;

				receiver.Finished -= OnTaskFinished;
				receiver.Stop();

				sender.Finished -= OnTaskFinished;
				sender.Stop();

				socket.SendMessage(new Message(Message.MSG_CLOSE));
				socket.Close();

				state = ConnectionState.Closed;
			}
			Closed.Raise();
		}

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			var message = new Message(RMSG_WDGMSG)
				.Uint16(widgetId)
				.String(name);
			if (args != null)
				message.List(args);
			sender.SendMessage(message);
		}

		public void RequestMapData(int x, int y)
		{
			var msg = new Message(Message.MSG_MAPREQ).Coord(x, y);
			socket.SendMessage(msg);
		}

		public void SendObjectAck(int id, int frame)
		{
			// FIXME: make it smarter
			socket.SendMessage(new Message(Message.MSG_OBJACK).Int32(id).Int32(frame));
		}

		private void Connect()
		{
			socket.Connect();

			var hello = new Message(Message.MSG_SESS)
				.Uint16(1)
				.String("Haven")
				.Uint16(ProtocolVersion)
				.String(settings.UserName)
				.Bytes(settings.Cookie);

			socket.SendMessage(hello);

			MessageReader reply;
			ConnectionError error;
			while (true)
			{
				if (!socket.Receive(out reply))
					throw new ConnectionException(ConnectionError.ConnectionError);

				if (reply.MessageType == Message.MSG_SESS)
				{
					error = (ConnectionError)reply.ReadByte();
					break;
				}
				if (reply.MessageType == Message.MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new ConnectionException(error);
		}

		private void ReceiveMessage(MessageReader msg)
		{
			switch (msg.MessageType)
			{
				case Message.MSG_REL:
					var seq = msg.ReadUint16();
					while (!msg.IsEom)
					{
						var type = msg.ReadByte();
						int len;
						if ((type & 0x80) != 0) // is not last?
						{
							type &= 0x7f;
							len = msg.ReadUint16();
						}
						else
							len = msg.Length - msg.Position;
						GotRel(seq, new MessageReader(type, msg, msg.Position, len));
						msg.Position += len;
						seq++;
					}
					break;
				case Message.MSG_ACK:
					sender.ReceiveAck(msg.ReadUint16());
					break;
				case Message.MSG_MAPDATA:
					HandleMapData(msg);
					break;
				case Message.MSG_OBJDATA:
					while (msg.Position < msg.Length)
					{
						var changeset = GobChangeset.ReadFrom(msg);
						listeners.ForEach(x => x.UpdateGob(changeset));
					}
					break;
				case Message.MSG_CLOSE:
					log.Info("Server dropped connection");
					Close();
					return;
			}
		}

		private void GotRel(ushort seq, MessageReader msg)
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

		private void HandleRel(MessageReader msg)
		{
			switch (msg.MessageType)
			{
				case RMSG_NEWWDG:
				{
					var args = CreateWidgetArgs.ReadFrom(msg);
					listeners.ForEach(x => x.CreateWidget(args));
					break;
				}
				case RMSG_WDGMSG:
				{
					var args = UpdateWidgetArgs.ReadFrom(msg);
					listeners.ForEach(x => x.UpdateWidget(args));
					break;
				}
				case RMSG_DSTWDG:
					var widgetId = msg.ReadUint16();
					listeners.ForEach(x => x.DestroyWidget(widgetId));
					break;
				case RMSG_MAPIV:
					listeners.ForEach(x => x.InvalidateMap());
					break;
				case RMSG_GLOBLOB:
				{
					while (!msg.IsEom)
					{
						switch (msg.ReadByte())
						{
							case GMSG_TIME:
								var time = msg.ReadInt32();
								break;
							case GMSG_ASTRO:
								int dt = msg.ReadInt32();
								int mp = msg.ReadInt32();
								int yt = msg.ReadInt32();
								double dtf = Defix(dt);
								double mpf = Defix(mp);
								double ytf = Defix(yt);
								var astronomy = new Astonomy(dtf, mpf);
								listeners.ForEach(x => x.UpdateAstronomy(astronomy));
								break;
							case GMSG_LIGHT:
								var amblight = msg.ReadColor();
								listeners.ForEach(x => x.UpdateAmbientLight(amblight));
								break;
						}
					}
					break;
				}
				case RMSG_PAGINAE:
					var actions = new List<ActionDelta>();
					while (!msg.IsEom)
					{
						actions.Add(new ActionDelta
						{
							RemoveFlag = msg.ReadByte() == '-',
							Name = msg.ReadString(),
							Version = msg.ReadUint16()
						});
					}
					listeners.ForEach(x => x.UpdateActions(actions));
					break;
				case RMSG_RESID:
				{
					var binding = ResourceBinding.ReadFrom(msg);
					listeners.ForEach(x => x.BindResource(binding));
					break;
				}
				case RMSG_PARTY:
					listeners.ForEach(x => x.UpdateParty());
					break;
				case RMSG_SFX:
					listeners.ForEach(x => x.PlaySound());
					break;
				case RMSG_CATTR:
					var attributes = new List<CharAttribute>();
					while (!msg.IsEom)
					{
						var name = msg.ReadString();
						var baseValue = msg.ReadInt32();
						var compValue = msg.ReadInt32();
						attributes.Add(new CharAttribute(name, baseValue, compValue));
					}
					listeners.ForEach(x => x.UpdateCharAttributes(attributes));
					break;
				case RMSG_MUSIC:
					listeners.ForEach(x => x.PlayMusic());
					break;
				case RMSG_TILES:
					var bindings = new List<TilesetBinding>();
					while (!msg.IsEom)
						bindings.Add(TilesetBinding.ReadFrom(msg));
					listeners.ForEach(x => x.BindTilesets(bindings));
					break;
				case RMSG_BUFF:
				{
					var message = msg.ReadString();
					switch (message)
					{
						case "clear":
							listeners.ForEach(x => x.ClearBuffs());
							break;
						case "rm":
							int id = msg.ReadInt32();
							listeners.ForEach(x => x.RemoveBuff(id));
							break;
						case "set":
							listeners.ForEach(x => x.AddBuff(
								new BuffData
								{
									Id = msg.ReadInt32(),
									ImageResId = msg.ReadUint16(),
									Tooltip = msg.ReadString(),
									AMeter = msg.ReadInt32(),
									NMeter = msg.ReadInt32(),
									CMeter = msg.ReadInt32(),
									CTicks = msg.ReadInt32(),
									Major = msg.ReadByte() != 0
								}));
							break;
					}
					break;
				}
				default:
					throw new Exception("Unknown rmsg type: " + msg.MessageType);
			}
		}

		private static double Defix(int i)
		{
			return i / 1e9;
		}

		// TODO: delete lingering fragments?
		private void HandleMapData(MessageReader msg)
		{
			int packetId = msg.ReadInt32();
			int offset = msg.ReadUint16();
			int length = msg.ReadUint16();

			FragmentBuffer fragbuf;
			if (mapFrags.Find(ref packetId, out fragbuf))
			{
				fragbuf.Add(offset, msg.Buffer, 8, msg.Length - 8);
				if (fragbuf.IsComplete)
				{
					mapFrags.Remove(packetId);
					var mapData = MapData.ReadFrom(new MessageReader(0, fragbuf.Content));
					listeners.ForEach(x => x.UpdateMap(mapData));
				}
			}
			else if (offset != 0 || msg.Length - 8 < length)
			{
				fragbuf = new FragmentBuffer(length);
				fragbuf.Add(offset, msg.Buffer, 8, msg.Length - 8);
				mapFrags[packetId] = fragbuf;
			}
			else
			{
				var mapData = MapData.ReadFrom(msg);
				listeners.ForEach(x => x.UpdateMap(mapData));
			}
		}

		private void OnTaskFinished(object sender, EventArgs args)
		{
			// it shouldn't happen normally, so let it crash
			throw new Exception("Task finished abruptly");
		}
	}
}
