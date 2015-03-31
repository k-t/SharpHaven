using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using C5;
using ICSharpCode.SharpZipLib.Zip.Compression;
using MonoHaven.Messages;
using MonoHaven.Resources;
using MonoHaven.Utils;
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
				// HACK: otherwise close message may not be sent
				Thread.Sleep(TimeSpan.FromMilliseconds(5));
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
						HandleObjData(msg);
					break;
				case Message.MSG_CLOSE:
					Log.Info("Server dropped connection");
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
					var args = WidgetCreateMessage.ReadFrom(msg);
					listeners.ForEach(x => x.CreateWidget(args));
					break;
				}
				case RMSG_WDGMSG:
				{
					var args = WidgetUpdateMessage.ReadFrom(msg);
					listeners.ForEach(x => x.UpdateWidget(args));
					break;
				}
				case RMSG_DSTWDG:
					var widgetId = msg.ReadUint16();
					listeners.ForEach(x => x.DestroyWidget(widgetId));
					break;
				case RMSG_MAPIV:
				{
					int type = msg.ReadByte();
					switch (type)
					{
						case 0:
							listeners.ForEach(x => x.InvalidateMap(msg.ReadCoord()));
							break;
						case 1:
							var ul = msg.ReadCoord();
							var br = msg.ReadCoord();
							listeners.ForEach(x => x.InvalidateMap(ul, br));
							break;
						case 2:
							listeners.ForEach(x => x.InvalidateMap());
							break;
					}
					break;
				}
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
								var astronomy = new AstronomyMessage {
									DayTime = Defix(dt),
									MoonPhase = Defix(mp)
								};
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
					var actions = new List<ActionMessage>();
					while (!msg.IsEom)
					{
						actions.Add(new ActionMessage
						{
							RemoveFlag = msg.ReadByte() == '-',
							Resource = new ResourceRef(msg.ReadString(), msg.ReadUint16())
						});
					}
					listeners.ForEach(x => x.UpdateActions(actions));
					break;
				case RMSG_RESID:
				{
					var message = new BindResourceMessage {
						Id = msg.ReadUint16(),
						Name = msg.ReadString(),
						Version = msg.ReadUint16()
					};
					listeners.ForEach(x => x.BindResource(message));
					break;
				}
				case RMSG_PARTY:
					while (!msg.IsEom)
					{
						int type = msg.ReadByte();
						switch (type)
						{
							case PD_LIST:
								var ids = new List<int>();
								while(true)
								{
									int id = msg.ReadInt32();
									if(id < 0)
										break;
									ids.Add(id);
									listeners.ForEach(x => x.UpdatePartyList(ids));
								}
								break;
							case PD_LEADER:
								var leaderId = msg.ReadInt32();
								listeners.ForEach(x => x.SetPartyLeader(leaderId));
								break;
							case PD_MEMBER:
								var memberId = msg.ReadInt32();
								var visible = msg.ReadByte() == 1;
								Point? location = null;
								if (visible)
									location = msg.ReadCoord();
								var color = msg.ReadColor();
								listeners.ForEach(x => x.UpdatePartyMember(memberId, color, location));
								break;
						}
					}
					break;
				case RMSG_SFX:
				{
					var message = new PlaySoundMessage {
						ResourceId = msg.ReadUint16(),
						Volume = msg.ReadUint16()/256.0,
						Speed = msg.ReadUint16()/256.0
					};
					listeners.ForEach(x => x.PlaySound(message));
					break;
				}
				case RMSG_CATTR:
					var attributes = new List<CharAttributeMessage>();
					while (!msg.IsEom)
					{
						var attribute = new CharAttributeMessage {
							Name = msg.ReadString(),
							BaseValue = msg.ReadInt32(),
							CompValue = msg.ReadInt32()
						};
						attributes.Add(attribute);
					}
					listeners.ForEach(x => x.UpdateCharAttributes(attributes));
					break;
				case RMSG_MUSIC:
					listeners.ForEach(x => x.PlayMusic());
					break;
				case RMSG_TILES:
					var messages = new List<BindTilesetMessage>();
					while (!msg.IsEom)
					{
						var message = new BindTilesetMessage {
							Id = msg.ReadByte(),
							Name = msg.ReadString(),
							Version = msg.ReadUint16()
						};
						messages.Add(message);
					}
					listeners.ForEach(x => x.BindTilesets(messages));
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
								new BuffAddMessage
								{
									Id = msg.ReadInt32(),
									ResId = msg.ReadUint16(),
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
					var mapData = GetMapData(new MessageReader(0, fragbuf.Content));
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
				var mapData = GetMapData(msg);
				listeners.ForEach(x => x.UpdateMap(mapData));
			}
		}

		public static UpdateMapMessage GetMapData(MessageReader reader)
		{
			var msg = new UpdateMapMessage {
				Grid = reader.ReadCoord(),
				MinimapName = reader.ReadString(),
				Tiles = new byte[100 * 100],
				Overlays = new int[100 * 100]
			};

			var pfl = new byte[256];
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = reader.ReadByte();
			}

			var blob = Unpack(reader.Buffer, reader.Position, reader.Length - reader.Position);
			Array.Copy(blob, msg.Tiles, msg.Tiles.Length);

			reader = new MessageReader(0, blob);
			reader.Position += msg.Tiles.Length;
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				int fl = pfl[pidx];
				int type = reader.ReadByte();
				var c1 = new Point(reader.ReadByte(), reader.ReadByte());
				var c2 = new Point(reader.ReadByte(), reader.ReadByte());

				int ol;
				if (type == 0)
					ol = ((fl & 1) == 1) ? 2 : 1;
				else if (type == 1)
					ol = ((fl & 1) == 1) ? 8 : 4;
				else
				{
					Log.Warn("Unknown plot type " + type);
					continue;
				}

				for (int y = c1.Y; y <= c2.Y; y++)
					for (int x = c1.X; x <= c2.X; x++)
						msg.Overlays[y * 100 + x] |= ol;
			}

			return msg;
		}

		private static byte[] Unpack(byte[] input, int offset, int length)
		{
			var buf = new byte[4096];
			var inflater = new Inflater();
			using (var output = new MemoryStream())
			{
				inflater.SetInput(input, offset, length);
				int n;
				while ((n = inflater.Inflate(buf)) != 0)
					output.Write(buf, 0, n);

				if (!inflater.IsFinished)
					throw new Exception("Got unterminated map blob");

				return output.ToArray();
			}
		}

		private void HandleObjData(MessageReader msg)
		{
			var gobData = new UpdateGobMessage();
			gobData.ReplaceFlag = (msg.ReadByte() & 1) != 0;
			gobData.GobId = msg.ReadInt32();
			gobData.Frame = msg.ReadInt32();
			while (true)
			{
				GobDelta delta;
				int type = msg.ReadByte();
				switch (type)
				{
					case OD_REM:
						delta = new GobDelta.Clear();
						break;
					case OD_MOVE:
						{
							var pos = msg.ReadCoord();
							delta = new GobDelta.Position { Value = pos };
							break;
						}
					case OD_RES:
						{
							int resId = msg.ReadUint16();
							byte[] spriteData;
							if ((resId & 0x8000) != 0)
							{
								resId &= ~0x8000;
								var len = msg.ReadByte();
								spriteData = msg.ReadBytes(len);
							}
							else
							{
								spriteData = new byte[0];
							}
							delta = new GobDelta.Resource { Id = resId, SpriteData = spriteData };
							break;
						}
					case OD_LINBEG:
						delta = new GobDelta.StartMovement
						{
							Origin = msg.ReadCoord(),
							Destination = msg.ReadCoord(),
							TotalSteps = msg.ReadInt32()
						};
						break;
					case OD_LINSTEP:
						delta = new GobDelta.AdjustMovement { Step = msg.ReadInt32() };
						break;
					case OD_SPEECH:
						delta = new GobDelta.Speech
						{
							Offset = msg.ReadCoord(),
							Text = msg.ReadString()
						};
						break;
					case OD_LAYERS:
					case OD_AVATAR:
						{
							int baseResId = -1;
							if (type == OD_LAYERS)
								baseResId = msg.ReadUint16();
							var layers = new List<int>();
							while (true)
							{
								int layer = msg.ReadUint16();
								if (layer == 65535)
									break;
								layers.Add(layer);
							}
							if (type == OD_LAYERS)
								delta = new GobDelta.Layers
								{
									BaseResourceId = baseResId,
									ResourceIds = layers.ToArray()
								};
							else
								delta = new GobDelta.Avatar { ResourceIds = layers.ToArray() };
							break;
						}
					case OD_DRAWOFF:
						delta = new GobDelta.DrawOffset { Value = msg.ReadCoord() };
						break;
					case OD_LUMIN:
						delta = new GobDelta.Light
						{
							Offset = msg.ReadCoord(),
							Size = msg.ReadUint16(),
							Intensity = msg.ReadByte()
						};
						break;
					case OD_FOLLOW:
						{
							int oid = msg.ReadInt32();
							if (oid != -1)
							{
								delta = new GobDelta.Follow
								{
									GobId = oid,
									Szo = msg.ReadByte(),
									Offset = msg.ReadCoord()
								};
							}
							else
								delta = new GobDelta.Follow { GobId = oid };
							break;
						}
					case OD_HOMING:
						{
							int oid = msg.ReadInt32();
							if (oid == -1)
								delta = new GobDelta.Homing { GobId = oid };
							else
								delta = new GobDelta.Homing
								{
									GobId = oid,
									Target = msg.ReadCoord(),
									Velocity = msg.ReadUint16()
								};
							break;
						}
					case OD_OVERLAY:
						{
							int overlayId = msg.ReadInt32();
							var prs = (overlayId & 1) != 0;
							overlayId >>= 1;
							int resId = msg.ReadUint16();
							byte[] spriteData = null;
							if (resId != 65535)
							{
								if ((resId & 0x8000) != 0)
								{
									resId &= ~0x8000;
									var len = msg.ReadByte();
									spriteData = msg.ReadBytes(len);
								}
								else
									spriteData = new byte[0];
							}
							delta = new GobDelta.Overlay
							{
								Id = overlayId,
								Prs = prs,
								ResourceId = resId,
								SpriteData = spriteData
							};
							break;
						}
					case OD_HEALTH:
						delta = new GobDelta.Health { Value = msg.ReadByte() };
						break;
					case OD_BUDDY:
						delta = new GobDelta.Buddy
						{
							Name = msg.ReadString(),
							Group = msg.ReadByte(),
							Type = msg.ReadByte()
						};
						break;
					case OD_END:
						delta = null;
						break;
					default:
						// TODO: MessageException
						throw new Exception("Unknown objdelta type: " + type);
				}
				if (delta == null)
					break;
				gobData.Deltas.Add(delta);
			}
			listeners.ForEach(x => x.UpdateGob(gobData));
			SendObjectAck(gobData.GobId, gobData.Frame);
		}

		private void OnTaskFinished(object sender, EventArgs args)
		{
			// it shouldn't happen normally, so let it crash
			throw new Exception("Task finished abruptly");
		}
	}
}
