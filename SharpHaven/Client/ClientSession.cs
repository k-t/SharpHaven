using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Events;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Resources;
using SharpHaven.UI.Remote;
using SharpHaven.Utils;

namespace SharpHaven.Client
{
	public class ClientSession : IGameEventListener
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly IGame game;
		private readonly HashSet<Point> gridRequests;
		private readonly ServerWidgetFactory widgetFactory;

		private readonly GameActionCollection actions;
		private readonly Dictionary<string, CharAttribute> attributes;
		private readonly BuffCollection buffs;
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;
		private readonly Party party;
		private readonly GameScreen screen;
		private readonly ServerWidgetCollection widgets;
		private readonly Dictionary<int, string> resources;

		public ClientSession(IGame game)
		{
			this.game = game;
			this.game.AddListener(this);

			gridRequests = new HashSet<Point>();
			widgetFactory = new ServerWidgetFactory();

			actions = new GameActionCollection();
			attributes = new Dictionary<string, CharAttribute>();
			buffs = new BuffCollection();
			map = new Map();
			objects = new GobCache();
			scene = new GameScene(this);
			party = new Party();
			screen = new GameScreen();

			widgets = new ServerWidgetCollection();
			widgets.Add(new ServerRootWidget(0, this, Screen.Root));

			resources = new Dictionary<int, string>();
		}

		public Map Map
		{
			get { return map; }
		}

		public Point WorldPosition
		{
			get;
			set;
		}

		public GameActionCollection Actions
		{
			get { return actions; }
		}

		public GobCache Objects
		{
			get { return objects; }
		}

		public GameScene Scene
		{
			get { return scene; }
		}

		public GameScreen Screen
		{
			get { return screen; }
		}

		public GameTime Time
		{
			get;
			set;
		}

		public Party Party
		{
			get { return party; }
		}

		public BuffCollection Buffs
		{
			get { return buffs; }
		}

		public ServerWidgetCollection Widgets
		{
			get { return widgets; }
		}

		public CharAttribute GetAttr(string name)
		{
			CharAttribute attr;
			return attributes.TryGetValue(name, out attr) ? attr : null;
		}

		public void SetAttr(string name, int baseValue, int compValue)
		{
			CharAttribute attr;
			if (attributes.TryGetValue(name, out attr))
				attr.Update(baseValue, compValue);
			else
			{
				attr = new CharAttribute(name, baseValue, compValue);
				attributes[name] = attr;
			}
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

		public void LoadResource(ushort resId, string resName)
		{
			resources[resId] = resName;
		}

		#endregion

		public void Start()
		{
			game.Start();
		}

		public void Finish()
		{
			lock (this)
			{
				game.RemoveListener(this);
				game.Stop();
			}
			App.QueueOnMainThread(() => Screen.Close());
		}

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			game.MessageWidget(widgetId, name, args);
		}

		public void RequestGrid(Point gc)
		{
			var grid = Map.GetGrid(gc);
			if (grid == null && !gridRequests.Contains(gc))
			{
				gridRequests.Add(gc);
				// TODO: Queue on sender thread?
				ThreadPool.QueueUserWorkItem(o => game.RequestMap(gc.X, gc.Y));
			}
		}

		#region IGameEventListener implementation

		void IGameEventListener.Handle(WidgetCreateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var parent = Widgets[args.ParentId];
				if (parent == null)
					throw new Exception(string.Format(
						"Non-existent parent widget {0} for {1}",
						args.ParentId,
						args.Id));

				var widget = widgetFactory.Create(args.Type, args.Id, parent);
				widget.Init(args.Position, args.Args);
				Widgets.Add(widget);
			});
		}

		void IGameEventListener.Handle(WidgetMessageEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var widget = Widgets[args.WidgetId];
				if (widget != null)
					widget.HandleMessage(args.Name, args.Args);
				else
					Log.Warn("UI message {1} to non-existent widget {0}",
						args.WidgetId, args.Name);
			});
		}

		void IGameEventListener.Handle(WidgetDestroyEvent args)
		{
			App.QueueOnMainThread(() => Widgets.Remove(args.WidgetId));
		}

		void IGameEventListener.Handle(ResourceLoadEvent args)
		{
			App.QueueOnMainThread(() => LoadResource(args.ResourceId, args.Name));
		}

		void IGameEventListener.Handle(TilesetsLoadEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in args.Tilesets)
					Map.BindTileset(binding);
			});
		}

		void IGameEventListener.Handle(MapInvalidateEvent args)
		{
			App.QueueOnMainThread(() => Map.InvalidateAll());
		}

		void IGameEventListener.Handle(MapInvalidateGridEvent args)
		{
			App.QueueOnMainThread(() => RequestGrid(args.Coord));
		}

		void IGameEventListener.Handle(MapInvalidateRegionEvent args)
		{
			App.QueueOnMainThread(() => Map.InvalidateRegion(args.Region));
		}

		void IGameEventListener.Handle(CharAttributesUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attr in args.Attributes)
					SetAttr(attr.Name, attr.BaseValue, attr.ModifiedValue);
			});
		}

		void IGameEventListener.Handle(GameTimeUpdateEvent args)
		{
			Log.Info("UpdateTime");
		}

		void IGameEventListener.Handle(AmbientLightUpdateEvent args)
		{
			Log.Info("UpdateAmbientLight");
		}

		void IGameEventListener.Handle(AstronomyUpdateEvent astonomy)
		{
			App.QueueOnMainThread(() => Time = new GameTime(astonomy.DayTime, astonomy.MoonPhase));
		}

		void IGameEventListener.Handle(GameActionsUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var action in args.Removed)
					Actions.Remove(action.Name);

				foreach (var action in args.Added)
					Actions.Add(action.Name);
			});
		}

		void IGameEventListener.Handle(PartyLeaderChangeEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				Party.LeaderId = args.LeaderId;
			});
		}

		void IGameEventListener.Handle(PartyUpdateEvent args)
		{
			App.QueueOnMainThread(() => Party.Update(args.MemberIds));
		}

		void IGameEventListener.Handle(PartyMemberUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var member = Party.GetMember(args.MemberId);
				if (member != null)
				{
					member.Color = args.Color;
					member.Location = args.Location;
				}
			});
		}

		void IGameEventListener.Handle(GobUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var updater = new GobUpdater(this);
				updater.ApplyChanges(args);
			});
		}

		void IGameEventListener.Handle(MapUpdateEvent message)
		{
			App.QueueOnMainThread(() =>
			{
				gridRequests.Remove(message.Grid);
				Map.AddGrid(message);
			});
		}

		void IGameEventListener.Handle(PlaySoundEvent args)
		{
			var res = GetResource(args.ResourceId);
			App.Audio.Play(res);
		}

		void IGameEventListener.Handle(PlayMusicEvent args)
		{
			Log.Info("PlayMusic");
		}

		void IGameEventListener.Exit()
		{
			App.QueueOnMainThread(Finish);
		}

		void IGameEventListener.Handle(BuffUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var buff = new Buff(args.Id, Get<BuffMold>(args.ResourceId));
				buff.Amount = args.AMeter;
				buff.IsMajor = args.IsMajor;
				if (!string.IsNullOrEmpty(args.Tooltip))
					buff.Tooltip = args.Tooltip;
				Buffs.AddOrUpdate(buff);
			});
		}

		void IGameEventListener.Handle(BuffRemoveEvent args)
		{
			App.QueueOnMainThread(() => Buffs.Remove(args.BuffId));
		}

		void IGameEventListener.Handle(BuffClearEvent args)
		{
			App.QueueOnMainThread(() => Buffs.Clear());
		}

		#endregion
	}
}
