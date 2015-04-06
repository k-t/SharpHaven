using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Messages;
using MonoHaven.Network;
using MonoHaven.Resources;
using MonoHaven.UI.Remote;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven.Game
{
	public class GameSession : IConnectionListener
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Connection connection;
		private readonly GameState state;
		private readonly Dictionary<int, string> resources;

		private readonly ServerWidgetFactory widgetFactory;
		private readonly ServerWidgetCollection widgets;

		public GameSession(ConnectionSettings connSettings)
		{
			state = new GameState(this);
			resources = new Dictionary<int, string>();
			widgetFactory = new ServerWidgetFactory();

			connection = new Connection(connSettings);
			connection.AddListener(this);

			widgets = new ServerWidgetCollection(this);
			widgets.Add(new ServerRootWidget(0, this, state.Screen.Root));
		}

		public GameState State
		{
			get { return state; }
		}

		public ServerWidgetCollection Widgets
		{
			get { return widgets; }
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
			App.QueueOnMainThread(() => state.Screen.Close());
		}

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			connection.SendMessage(widgetId, name, args);
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
			string resName;
			return resources.TryGetValue(id, out resName)
				? new Delayed<T>(getter(resName))
				: new Delayed<T>((out T res) =>
				{
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
			App.QueueOnMainThread(() =>
			{
				var parent = widgets.Get(message.ParentId);
				if (parent == null)
					throw new Exception(string.Format(
						"Non-existent parent widget {0} for {1}",
						message.ParentId,
						message.Id));

				var widget = widgetFactory.Create(message.Type, message.Id, parent);
				widget.Init(message.Position, message.Args);
				widgets.Add(widget);
			});
		}

		void IConnectionListener.UpdateWidget(WidgetUpdateMessage message)
		{
			App.QueueOnMainThread(() =>
			{
				var widget = widgets.Get(message.Id);
				if (widget != null)
					widget.HandleMessage(message.Name, message.Args);
				else
					Log.Warn("UI message {1} to non-existent widget {0}",
						message.Id, message.Name);
			});
		}

		void IConnectionListener.DestroyWidget(ushort widgetId)
		{
			App.QueueOnMainThread(() => widgets.Remove(widgetId));
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
			App.QueueOnMainThread(() => State.Map.InvalidateAll());
		}

		void IConnectionListener.InvalidateMap(Point gc)
		{
			App.QueueOnMainThread(() => State.Map.Invalidate(gc));
		}

		void IConnectionListener.InvalidateMap(Point ul, Point br)
		{
			App.QueueOnMainThread(() => State.Map.InvalidateRange(ul, br));
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
			Log.Info("UpdateTime");
		}

		void IConnectionListener.UpdateAmbientLight(Color color)
		{
			Log.Info("UpdateAmbientLight");
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

		void IConnectionListener.SetPartyLeader(int leaderId)
		{
			App.QueueOnMainThread(() =>
			{
				State.Party.LeaderId = leaderId;
			});
		}

		void IConnectionListener.UpdatePartyList(List<int> memberIds)
		{
			App.QueueOnMainThread(() => State.Party.Update(memberIds));
		}

		void IConnectionListener.UpdatePartyMember(int memberId, Color color, Point? location)
		{
			App.QueueOnMainThread(() =>
			{
				var member = State.Party.GetMember(memberId);
				if (member != null)
				{
					member.Color = color;
					member.Location = location;
				}
			});
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
			Log.Info("PlayMusic");
		}

		void IConnectionListener.Exit()
		{
			App.QueueOnMainThread(Finish);
		}

		void IConnectionListener.AddBuff(BuffAddMessage message)
		{
			App.QueueOnMainThread(() =>
			{
				var buff = new Buff(message.Id, Get<BuffMold>(message.ResId));
				buff.Amount = message.AMeter;
				buff.IsMajor = message.Major;
				if (!string.IsNullOrEmpty(message.Tooltip))
					buff.Tooltip = message.Tooltip;
				State.UpdateBuff(buff);
			});
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
