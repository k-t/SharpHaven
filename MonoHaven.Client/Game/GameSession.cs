using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Messages;
using MonoHaven.Network;
using MonoHaven.Resources;
using MonoHaven.Utils;
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
			screen.Close();
		}

		#region Resource Management

		public Delayed<Resource> GetResource(int id)
		{
			return GetResource(id, App.Resources.Load);
		}

		public Delayed<ISprite> GetSprite(int id, byte[] spriteState = null)
		{
			return GetResource(id, n => App.Resources.GetSprite(n, spriteState));
		}

		public Delayed<T> Get<T>(int id) where T : class 
		{
			return GetResource(id, n => App.Resources.Get<T>(n));
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

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			connection.SendMessage(widgetId, name, args);
		}

		#endregion

		#region Map Data Management

		public void RequestData(int x, int y)
		{
			// TODO: Queue on sender thread?
			ThreadPool.QueueUserWorkItem(o => connection.RequestMapData(x, y));
		}

		#endregion

		#region IConnectionListener implementation

		void IConnectionListener.CreateWidget(WidgetCreateMessage message)
		{
			App.QueueOnMainThread(() => screen.CreateWidget(message));
		}

		void IConnectionListener.UpdateWidget(WidgetUpdateMessage message)
		{
			App.QueueOnMainThread(() => screen.UpdateWidget(message));
		}

		void IConnectionListener.DestroyWidget(ushort widgetId)
		{
			App.QueueOnMainThread(() => screen.DestroyWidget(widgetId));
		}

		void IConnectionListener.BindResource(BindResourceMessage message)
		{
			App.QueueOnMainThread(() => resources[message.Id] = message.Name);
		}

		void IConnectionListener.BindTilesets(IEnumerable<BindTilesetMessage> bindings)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in bindings)
					State.Map.BindTileset(binding);
			});
		}

		void IConnectionListener.InvalidateMap()
		{
			log.Info("InvalidateMap");
		}

		void IConnectionListener.UpdateCharAttributes(IEnumerable<CharAttributeMessage> attributes)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attr in attributes)
					State.SetAttr(attr.Name, attr.BaseValue, attr.CompValue);
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

		void IConnectionListener.UpdateAstronomy(AstronomyMessage astonomy)
		{
			App.QueueOnMainThread(() => state.Time = new GameTime(astonomy.DayTime, astonomy.MoonPhase));
		}

		void IConnectionListener.UpdateActions(IEnumerable<ActionMessage> actions)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var action in actions)
					if (action.RemoveFlag)
						State.Actions.Remove(action.Resource.Name);
					else
						State.Actions.Add(action.Resource.Name);
			});
		}

		void IConnectionListener.UpdateParty()
		{
			log.Info("UpdateParty");
		}

		void IConnectionListener.UpdateGob(UpdateGobMessage message)
		{
			App.QueueOnMainThread(() =>
			{
				var updater = new GobUpdater(this);
				updater.ApplyChanges(message);
			});
		}

		void IConnectionListener.UpdateMap(UpdateMapMessage updateMapMessage)
		{
			App.QueueOnMainThread(() => State.Map.AddGrid(updateMapMessage));
		}

		void IConnectionListener.PlaySound(PlaySoundMessage message)
		{
			var res = GetResource(message.ResourceId);
			App.Audio.Play(res);
		}

		void IConnectionListener.PlayMusic()
		{
			log.Info("PlayMusic");
		}

		void IConnectionListener.Exit()
		{
			App.QueueOnMainThread(Finish);
		}

		void IConnectionListener.AddBuff(BuffAddMessage message)
		{
			App.QueueOnMainThread(() => State.AddBuff(message));
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
