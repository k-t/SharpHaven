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
		private readonly ClientState state;
		private readonly Dictionary<int, string> resources;
		private readonly HashSet<Point> gridRequests;

		private readonly ServerWidgetFactory widgetFactory;
		private readonly ServerWidgetCollection widgets;

		public ClientSession(ClientState state, IGame game)
		{
			this.state = state;

			this.game = game;
			this.game.AddListener(this);

			gridRequests = new HashSet<Point>();
			resources = new Dictionary<int, string>();
			widgetFactory = new ServerWidgetFactory();

			widgets = new ServerWidgetCollection(this);
			widgets.Add(new ServerRootWidget(0, this, state.Screen.Root));
		}

		public ClientState State
		{
			get { return state; }
		}

		public ServerWidgetCollection Widgets
		{
			get { return widgets; }
		}

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
			App.QueueOnMainThread(() => state.Screen.Close());
		}

		public void SendMessage(ushort widgetId, string name, object[] args)
		{
			game.MessageWidget(widgetId, name, args);
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
				ThreadPool.QueueUserWorkItem(o => game.RequestMap(gc.X, gc.Y));
			}
		}

		#region IGameEventListener implementation

		void IGameEventListener.Handle(WidgetCreateEvent args)
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

		void IGameEventListener.Handle(WidgetMessageEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var widget = widgets.Get(args.WidgetId);
				if (widget != null)
					widget.HandleMessage(args.Name, args.Args);
				else
					Log.Warn("UI message {1} to non-existent widget {0}",
						args.WidgetId, args.Name);
			});
		}

		void IGameEventListener.Handle(WidgetDestroyEvent args)
		{
			App.QueueOnMainThread(() => widgets.Remove(args.WidgetId));
		}

		void IGameEventListener.Handle(ResourceLoadEvent args)
		{
			App.QueueOnMainThread(() => resources[args.ResourceId] = args.Name);
		}

		void IGameEventListener.Handle(TilesetsLoadEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in args.Tilesets)
					State.Map.BindTileset(binding);
			});
		}

		void IGameEventListener.Handle(MapInvalidateEvent args)
		{
			App.QueueOnMainThread(() => State.Map.InvalidateAll());
		}

		void IGameEventListener.Handle(MapInvalidateGridEvent args)
		{
			App.QueueOnMainThread(() => RequestGrid(args.Coord));
		}

		void IGameEventListener.Handle(MapInvalidateRegionEvent args)
		{
			App.QueueOnMainThread(() => State.Map.InvalidateRegion(args.Region));
		}

		void IGameEventListener.Handle(CharAttributesUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attr in args.Attributes)
					State.SetAttr(attr.Name, attr.BaseValue, attr.ModifiedValue);
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
			App.QueueOnMainThread(() => state.Time = new GameTime(astonomy.DayTime, astonomy.MoonPhase));
		}

		void IGameEventListener.Handle(GameActionsUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var action in args.Removed)
					State.Actions.Remove(action.Name);

				foreach (var action in args.Added)
					State.Actions.Add(action.Name);
			});
		}

		void IGameEventListener.Handle(PartyLeaderChangeEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				State.Party.LeaderId = args.LeaderId;
			});
		}

		void IGameEventListener.Handle(PartyUpdateEvent args)
		{
			App.QueueOnMainThread(() => State.Party.Update(args.MemberIds));
		}

		void IGameEventListener.Handle(PartyMemberUpdateEvent args)
		{
			App.QueueOnMainThread(() =>
			{
				var member = State.Party.GetMember(args.MemberId);
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
				State.Map.AddGrid(message);
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
				State.UpdateBuff(buff);
			});
		}

		void IGameEventListener.Handle(BuffRemoveEvent args)
		{
			App.QueueOnMainThread(() => State.RemoveBuff(args.BuffId));
		}

		void IGameEventListener.Handle(BuffClearEvent args)
		{
			App.QueueOnMainThread(() => State.ClearBuffs());
		}

		#endregion
	}
}
