using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using NLog;
using SharpHaven.Game.Events;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Network;
using SharpHaven.Resources;
using SharpHaven.UI.Remote;
using SharpHaven.Utils;

namespace SharpHaven.Game
{
	public class GameSession : IGameEventListener
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Connection connection;
		private readonly GameState state;
		private readonly Dictionary<int, string> resources;
		private readonly HashSet<Point> gridRequests;

		private readonly ServerWidgetFactory widgetFactory;
		private readonly ServerWidgetCollection widgets;

		public GameSession(GameState state, ConnectionSettings connSettings)
		{
			this.state = state;

			connection = new Connection(connSettings);
			connection.AddListener(this);

			gridRequests = new HashSet<Point>();
			resources = new Dictionary<int, string>();
			widgetFactory = new ServerWidgetFactory();

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

		public void RequestGrid(Point gc)
		{
			var grid = State.Map.GetGrid(gc);
			if (grid == null && !gridRequests.Contains(gc))
			{
				gridRequests.Add(gc);
				// TODO: Queue on sender thread?
				ThreadPool.QueueUserWorkItem(o => connection.RequestMapData(gc.X, gc.Y));
			}
		}

		#region IGameEventListener implementation

		void IGameEventListener.CreateWidget(WidgetCreateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var parent = widgets.Get(args.ParentId);
				if (parent == null)
					throw new Exception(string.Format(
						"Non-existent parent widget {0} for {1}",
						args.ParentId,
						args.Id));

				var widget = widgetFactory.Create(args.Type, args.Id, parent);
				widget.Init(args.Position, args.Args);
				widgets.Add(widget);
			});
		}

		void IGameEventListener.UpdateWidget(WidgetMessageEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var widget = widgets.Get(args.Id);
				if (widget != null)
					widget.HandleMessage(args.Name, args.Args);
				else
					Log.Warn("UI message {1} to non-existent widget {0}",
						args.Id, args.Name);
			});
		}

		void IGameEventListener.DestroyWidget(ushort widgetId)
		{
			App.QueueOnMainThread(() => widgets.Remove(widgetId));
		}

		void IGameEventListener.LoadResource(ResourceLoadEvent args)
		{
			App.QueueOnMainThread(() => resources[args.Id] = args.Name);
		}

		void IGameEventListener.LoadTilesets(IEnumerable<TilesetLoadEvent> args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in args)
					State.Map.BindTileset(binding);
			});
		}

		void IGameEventListener.InvalidateMap()
		{
			App.QueueOnMainThread(() => State.Map.InvalidateAll());
		}

		void IGameEventListener.InvalidateMap(Point gc)
		{
			App.QueueOnMainThread(() => RequestGrid(gc));
		}

		void IGameEventListener.InvalidateMap(Point ul, Point br)
		{
			App.QueueOnMainThread(() => State.Map.InvalidateRange(ul, br));
		}

		void IGameEventListener.UpdateCharAttributes(IEnumerable<CharAttrUpdateEvent> attributes)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attr in attributes)
					State.SetAttr(attr.Name, attr.BaseValue, attr.CompValue);
			});
		}

		void IGameEventListener.UpdateTime(int time)
		{
			Log.Info("UpdateTime");
		}

		void IGameEventListener.UpdateAmbientLight(Color color)
		{
			Log.Info("UpdateAmbientLight");
		}

		void IGameEventListener.UpdateAstronomy(AstronomyEvent astonomy)
		{
			App.QueueOnMainThread(() => state.Time = new GameTime(astonomy.DayTime, astonomy.MoonPhase));
		}

		void IGameEventListener.UpdateActions(IEnumerable<ActionUpdateEvent> actions)
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

		void IGameEventListener.SetPartyLeader(int leaderId)
		{
			App.QueueOnMainThread(() =>
			{
				State.Party.LeaderId = leaderId;
			});
		}

		void IGameEventListener.UpdatePartyList(List<int> memberIds)
		{
			App.QueueOnMainThread(() => State.Party.Update(memberIds));
		}

		void IGameEventListener.UpdatePartyMember(int memberId, Color color, Point? location)
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

		void IGameEventListener.UpdateGob(GobUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var updater = new GobUpdater(this);
				updater.ApplyChanges(args);
			});
		}

		void IGameEventListener.UpdateMap(MapUpdateEvent message)
		{
			App.QueueOnMainThread(() =>
			{
				gridRequests.Remove(message.Grid);
				State.Map.AddGrid(message);
			});
		}

		void IGameEventListener.PlaySound(SoundEvent args)
		{
			var res = GetResource(args.ResourceId);
			App.Audio.Play(res);
		}

		void IGameEventListener.PlayMusic()
		{
			Log.Info("PlayMusic");
		}

		void IGameEventListener.Exit()
		{
			App.QueueOnMainThread(Finish);
		}

		void IGameEventListener.UpdateBuff(BuffUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var buff = new Buff(args.Id, Get<BuffMold>(args.ResId));
				buff.Amount = args.AMeter;
				buff.IsMajor = args.Major;
				if (!string.IsNullOrEmpty(args.Tooltip))
					buff.Tooltip = args.Tooltip;
				State.UpdateBuff(buff);
			});
		}

		void IGameEventListener.RemoveBuff(int buffId)
		{
			App.QueueOnMainThread(() => State.RemoveBuff(buffId));
		}

		void IGameEventListener.ClearBuffs()
		{
			App.QueueOnMainThread(() => State.ClearBuffs());
		}

		#endregion
	}
}
