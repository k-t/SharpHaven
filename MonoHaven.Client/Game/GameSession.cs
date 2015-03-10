using System;
using System.Collections.Generic;
using System.Threading;
using MonoHaven.Game.Messages;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Network;
using MonoHaven.Resources;
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
			connection.GobDataReceived += OnGobDataReceived;
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
					App.QueueOnMainThread(() => {
						var msg = WidgetCreateMessage.ReadFrom(messageReader);
						WidgetCreated.Raise(msg);
					});
					break;
				case RMSG_WDGMSG:
					App.QueueOnMainThread(() => {
						var msg = Messages.WidgetMessage.ReadFrom(messageReader);
						WidgetMessage.Raise(msg);
					});
					break;
				case RMSG_DSTWDG:
					App.QueueOnMainThread(() => {
						var msg = WidgetDestroyMessage.ReadFrom(messageReader);
						WidgetDestroyed.Raise(msg);
					});
					break;
				case RMSG_MAPIV:
					log.Info("RMSG_MAPIV");
					break;
				case RMSG_GLOBLOB:
					App.QueueOnMainThread(() => {
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
					App.QueueOnMainThread(() =>
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
					App.QueueOnMainThread(() =>
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
					App.QueueOnMainThread(() =>
					{
						while (!messageReader.IsEom)
						{
							var name = messageReader.ReadString();
							var baseValue = messageReader.ReadInt32();
							var compValue = messageReader.ReadInt32();
							State.SetAttr(name, baseValue, compValue);
						}
					});
					break;
				case RMSG_MUSIC:
					log.Info("RMSG_MUSIC");
					break;
				case RMSG_TILES:
					App.QueueOnMainThread(() =>
					{
						while (!messageReader.IsEom)
						{
							var msg = TilesetBinding.ReadFrom(messageReader);
							TilesetBound.Raise(msg);
						}
					});
					break;
				case RMSG_BUFF:
					App.QueueOnMainThread(() =>
					{
						var message = messageReader.ReadString();
						switch (message)
						{
							case "clear":
								State.ClearBuffs();
								break;
							case "rm":
								int id = messageReader.ReadInt32();
								State.RemoveBuff(id);
								break;
							case "set":
								State.AddBuff(new Buff {
									Id = messageReader.ReadInt32(),
									Image = GetImage(messageReader.ReadUint16()),
									Tooltip = messageReader.ReadString(),
									AMeter = messageReader.ReadInt32(),
									NMeter = messageReader.ReadInt32(),
									CMeter = messageReader.ReadInt32(),
									CTicks = messageReader.ReadInt32(),
									Major = messageReader.ReadByte() != 0
								});
								break;
						}
					});
					break;
				default:
					throw new Exception("Unknown rmsg type: " + messageReader.MessageType);
			}
		}

		private void OnMapDataReceived(MessageReader msg)
		{
			var mapData = MapData.ReadFrom(msg);
			App.QueueOnMainThread(() => MapDataAvailable.Raise(mapData));
		}

		private void OnGobDataReceived(MessageReader msg)
		{
			var gobData = GobData.ReadFrom(msg);
			App.QueueOnMainThread(() =>
			{
				var changeset = new GobChangeset(this, gobData.GobId, gobData.Frame, gobData.ReplaceFlag);
				foreach (var delta in gobData.Deltas)
					delta.Visit(changeset);
				changeset.Apply();
			});
			connection.SendObjectAck(gobData.GobId, gobData.Frame);
		}

		private static double Defix(int i)
		{
			return i / 1e9;
		}

		#region Resource Management

		public Delayed<Resource> GetResource(int id)
		{
			return GetResource(id, App.Resources.Get);
		}

		public Delayed<ISprite> GetSprite(int id, byte[] spriteState = null)
		{
			return GetResource(id, n => App.Resources.GetSprite(n, spriteState));
		}

		public Delayed<Drawable> GetImage(int id)
		{
			return GetResource(id, n => App.Resources.GetImage(n));
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
