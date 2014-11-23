﻿using System;
using System.Collections.Generic;
using System.Threading;
using MonoHaven.Game.Messages;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Network;
using MonoHaven.Resources;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven.Game
{
	public class GameSession
	{
		#region Constants

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

		private const int GMSG_TIME = 0;
		private const int GMSG_ASTRO = 1;
		private const int GMSG_LIGHT = 2;

		#endregion

		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		private readonly Connection connection;
		private readonly GameState state;
		private readonly GameScreen screen;
		private readonly Dictionary<int, string> resources;

		public GameSession(ConnectionSettings connSettings)
		{
			connection = new Connection(connSettings);
			connection.MessageReceived += OnMessageReceived;
			connection.MapDataReceived += OnMapDataReceived;
			connection.ObjDataReceived += OnObjDataReceived;
			connection.Closed += OnConnectionClosed;

			state = new GameState(this);
			screen = new GameScreen(this);
			resources = new Dictionary<int, string>();
		}

		public event Action Finished;

		public GameScreen Screen
		{
			get { return screen; }
		}

		public GameState State
		{
			get { return state; }
		}

		public void Start()
		{
			connection.Open();
		}

		public void Finish()
		{
			lock (this)
			{
				connection.Closed -= OnConnectionClosed;
				connection.Close();
			}
			Finished.Raise();
		}

		private void OnConnectionClosed()
		{
			Finish();
		}

		private void OnMessageReceived(MessageReader messageReader)
		{
			switch (messageReader.MessageType)
			{
				case RMSG_NEWWDG:
					App.Instance.QueueOnMainThread(() => {
						var msg = WidgetCreateMessage.ReadFrom(messageReader);
						WidgetCreated.Raise(msg);
					});
					break;
				case RMSG_WDGMSG:
					App.Instance.QueueOnMainThread(() => {
						var msg = Messages.WidgetMessage.ReadFrom(messageReader);
						WidgetMessage.Raise(msg);
					});
					break;
				case RMSG_DSTWDG:
					App.Instance.QueueOnMainThread(() => {
						var msg = WidgetDestroyMessage.ReadFrom(messageReader);
						WidgetDestroyed.Raise(msg);
					});
					break;
				case RMSG_MAPIV:
					log.Info("RMSG_MAPIV");
					break;
				case RMSG_GLOBLOB:
					App.Instance.QueueOnMainThread(() => {
						while (!messageReader.IsEom)
						{
							switch (messageReader.ReadByte())
							{
								case GMSG_TIME:
									var time = messageReader.ReadInt32();
									break;
								case GMSG_ASTRO:
									int dt = messageReader.ReadInt32();
									int mp = messageReader.ReadInt32();
									int yt = messageReader.ReadInt32();
									double dtf = Defix(dt);
									double mpf = Defix(mp);
									double ytf = Defix(yt);
									state.Time = new GameTime(dtf, mpf);
									break;
								case GMSG_LIGHT:
									var amblight = messageReader.ReadColor();
									break;
							}
						}
					});
					break;
				case RMSG_PAGINAE:
					App.Instance.QueueOnMainThread(() =>
					{
						while (!messageReader.IsEom)
						{
							var act = (char)messageReader.ReadByte();
							if (act == '+')
							{
								var name = messageReader.ReadString();
								var ver = messageReader.ReadUint16();
								state.Actions.Add(name);
							}
							else if (act == '-')
							{
								var name = messageReader.ReadString();
								var ver = messageReader.ReadUint16();
								state.Actions.Remove(name);
							}
						}
					});
					break;
				case RMSG_RESID:
					App.Instance.QueueOnMainThread(() =>
					{
						var id = messageReader.ReadUint16();
						var name = messageReader.ReadString();
						var ver = messageReader.ReadUint16();
						BindResource(id, name, ver);
					});
					break;
				case RMSG_PARTY:
					log.Info("RMSG_PARTY");
					break;
				case RMSG_SFX:
					log.Info("RMSG_SFX");
					break;
				case RMSG_CATTR:
					log.Info("RMSG_CATTR");
					break;
				case RMSG_MUSIC:
					log.Info("RMSG_MUSIC");
					break;
				case RMSG_TILES:
					App.Instance.QueueOnMainThread(() =>
					{
						while (!messageReader.IsEom)
						{
							var msg = TilesetBinding.ReadFrom(messageReader);
							TilesetBound.Raise(msg);
						}
					});
					break;
				case RMSG_BUFF:
					log.Info("RMSG_BUFF");
					break;
				default:
					throw new Exception("Unknown rmsg type: " + messageReader.MessageType);
			}
		}

		private void OnMapDataReceived(MessageReader msg)
		{
			var mapData = MapData.ReadFrom(msg);
			App.Instance.QueueOnMainThread(() => MapDataAvailable.Raise(mapData));
		}

		private void OnObjDataReceived(MessageReader msg)
		{
			var replace = (msg.ReadByte() & 1) != 0;
			var id = msg.ReadInt32();
			var frame = msg.ReadInt32();
			var delta = new GobDelta(this, id, frame, replace);
			var end = false;
			while (!end)
			{
				int type = msg.ReadByte();
				switch (type)
				{
					case OD_REM:
						delta.Remove();
						break;
					case OD_MOVE:
					{
						var pos = msg.ReadCoord();
						delta.SetPosition(pos);
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
						delta.SetResource(resId, spriteData);
						break;
					}
					case OD_LINBEG:
					{
						var orig = msg.ReadCoord();
						var dest = msg.ReadCoord();
						var time = msg.ReadInt32();
						delta.MoveStart(orig, dest, time);
						break;
					}
					case OD_LINSTEP:
					{
						var time = msg.ReadInt32();
						delta.MoveAdjust(time);
						break;
					}
					case OD_SPEECH:
					{
						var offset = msg.ReadCoord();
						var text = msg.ReadString();
						delta.SetSpeech(offset, text);
						break;
					}
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
							delta.SetLayers(baseResId, layers);
						else
							delta.SetAvatar(layers);
						break;
					}
					case OD_DRAWOFF:
					{
						var offset = msg.ReadCoord();
						delta.SetDrawOffset(offset);
						break;
					}
					case OD_LUMIN:
					{
						var offset = msg.ReadCoord();
						var size = msg.ReadUint16();
						var intensity = msg.ReadByte();
						delta.SetLight(offset, size, intensity);
						break;
					}
					case OD_FOLLOW:
					{
						int oid = msg.ReadInt32();
						if (oid != -1)
						{
							var szo = msg.ReadByte();
							var offset = msg.ReadCoord();
							delta.SetFollow(oid, szo, offset);
						}
						else
							delta.ResetFollow();
						break;
					}
					case OD_HOMING:
					{
						int oid = msg.ReadInt32();
						if (oid != -1)
							delta.ResetHoming();
						else
						{
							var target = msg.ReadCoord();
							var velocity = msg.ReadUint16();
							if (oid == -2)
								delta.SetHoming(target, velocity);
							else
								delta.SetHoming(oid, target, velocity);
						}
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
						delta.SetOverlay(overlayId, prs, resId, spriteData);
						break;
					}
					case OD_HEALTH:
					{
						var hp = msg.ReadByte();
						delta.SetHealth(hp);
						break;
					}
					case OD_BUDDY:
					{
						var name = msg.ReadString();
						var group = msg.ReadByte();
						var btype = msg.ReadByte();
						delta.SetBuddy(name, group, btype);
						break;
					}
					case OD_END:
						end = true;
						break;
					default:
						// TODO: MessageException
						throw new Exception("Unknown objdelta type: " + type);
				}
			}
			App.Instance.QueueOnMainThread(delta.Apply);
			connection.SendObjectAck(id, frame);
		}

		private static double Defix(int i)
		{
			return i / 1e9;
		}

		#region Resource Management

		public Delayed<Resource> GetResource(int id)
		{
			return GetResource(id, App.Instance.Resources.Get);
		}

		public Delayed<ISprite> GetSprite(int id, byte[] spriteState = null)
		{
			return GetResource(id, nm => App.Instance.Resources.GetSprite(nm, spriteState));
		}

		private Delayed<T> GetResource<T>(int id, Func<string, T> getter)
			where T : class
		{
			return new Delayed<T>((out T res) =>
			{
				string resName;
				if (resources.TryGetValue(id, out resName))
				{
					res = getter(resName);
					return true;
				}
				res = null;
				return false;
			});
		}

		private void BindResource(int id, string name, int version)
		{
			resources[id] = name;
		}

		#endregion

		#region Widget Management

		public event Action<WidgetCreateMessage> WidgetCreated;
		public event Action<WidgetDestroyMessage> WidgetDestroyed;
		public event Action<WidgetMessage> WidgetMessage;

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			var message = new Message(RMSG_WDGMSG)
				.Uint16(widgetId)
				.String(name);
			if (args != null)
				message.List(args);
			connection.SendMessage(message);
		}

		#endregion

		#region Map Data Management

		public event Action<MapData> MapDataAvailable;
		public event Action<TilesetBinding> TilesetBound;

		public void RequestData(int x, int y)
		{
			// TODO: Queue on sender thread?
			ThreadPool.QueueUserWorkItem(o => connection.RequestMapData(x, y));
		}

		#endregion
	}
}
