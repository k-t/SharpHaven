using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Network;
using MonoHaven.Network.Messages;
using MonoHaven.Resources;
using NLog;

namespace MonoHaven.Game
{
	public class GameSession : IConnectionListener
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		private readonly Connection connection;
		private readonly GameState state;
		private readonly GameScreen screen;
		private readonly Dictionary<int, string> resources;

		public GameSession(ConnectionSettings connSettings)
		{
			connection = new Connection(connSettings);
			connection.AddListener(this);

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
				connection.RemoveListener(this);
				connection.Close();
			}
			Finished.Raise();
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

		#endregion

		#region Widget Management

		public event Action<CreateWidgetArgs> WidgetCreated;
		public event Action<ushort> WidgetDestroyed;
		public event Action<UpdateWidgetArgs> WidgetMessage;

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			connection.SendMessage(widgetId, name, args);
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

		#region IConnectionListener implementation

		void IConnectionListener.CreateWidget(CreateWidgetArgs args)
		{
			App.QueueOnMainThread(() => WidgetCreated.Raise(args));
		}

		void IConnectionListener.UpdateWidget(UpdateWidgetArgs args)
		{
			App.QueueOnMainThread(() => WidgetMessage.Raise(args));
		}

		void IConnectionListener.DestroyWidget(ushort widgetId)
		{
			App.QueueOnMainThread(() => WidgetDestroyed.Raise(widgetId));
		}

		void IConnectionListener.BindResource(ResourceBinding binding)
		{
			App.QueueOnMainThread(() => resources[binding.Id] = binding.Name);
		}

		void IConnectionListener.BindTilesets(IEnumerable<TilesetBinding> bindings)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in bindings)
					TilesetBound.Raise(binding);
			});
		}

		void IConnectionListener.InvalidateMap()
		{
			log.Info("InvalidateMap");
		}

		void IConnectionListener.UpdateCharAttributes(IEnumerable<CharAttribute> attributes)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attribute in attributes)
					State.SetAttr(attribute);
			});
		}

		void IConnectionListener.UpdateTime(int time)
		{
			log.Info("UpdateTime");
		}

		void IConnectionListener.UpdateAmbientLight(Color color)
		{
			log.Info("UpdateAmbientLight");
		}

		void IConnectionListener.UpdateAstronomy(Astonomy astonomy)
		{
			App.QueueOnMainThread(() => state.Astronomy = astonomy);
		}

		void IConnectionListener.UpdateActions(IEnumerable<ActionDelta> actions)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var action in actions)
					if (action.RemoveFlag)
						State.Actions.Remove(action.Name);
					else
						State.Actions.Add(action.Name);
			});
		}

		void IConnectionListener.UpdateParty()
		{
			log.Info("UpdateParty");
		}

		void IConnectionListener.UpdateGob(GobChangeset changeset)
		{
			App.QueueOnMainThread(() =>
			{
				var updater = new GobUpdater(this);
				updater.ApplyChanges(changeset);
			});
		}

		void IConnectionListener.UpdateMap(MapData mapData)
		{
			App.QueueOnMainThread(() => MapDataAvailable.Raise(mapData));
		}

		void IConnectionListener.PlaySound()
		{
			log.Info("PlaySound");
		}

		void IConnectionListener.PlayMusic()
		{
			log.Info("PlayMusic");
		}

		void IConnectionListener.Exit()
		{
			App.QueueOnMainThread(Finish);
		}

		void IConnectionListener.AddBuff(BuffData buffData)
		{
			App.QueueOnMainThread(() => State.AddBuff(buffData));
		}

		void IConnectionListener.RemoveBuff(int buffId)
		{
			App.QueueOnMainThread(() => State.RemoveBuff(buffId));
		}

		void IConnectionListener.ClearBuffs()
		{
			App.QueueOnMainThread(() => State.ClearBuffs());
		}

		#endregion
	}
}
