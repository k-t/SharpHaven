using System;
using System.Collections.Generic;
using System.Threading;
using NLog;
using SharpHaven.Game;
using SharpHaven.Game.Messages;
using SharpHaven.Graphics;
using SharpHaven.Net;
using SharpHaven.UI.Remote;

namespace SharpHaven.Client
{
	public class ClientSession : DefaultMessageHandler
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly GameClient client;
		private readonly HashSet<Coord2D> gridRequests;
		private readonly ServerWidgetFactory widgetFactory;

		public ClientSession(GameClient client) :
			base(client.Messages)
		{
			this.client = client;

			gridRequests = new HashSet<Coord2D>();
			widgetFactory = new ServerWidgetFactory();

			Actions = new GameActionCollection();
			Attributes = new CharAttributeCollection();
			Buffs = new BuffCollection();
			Map = new Map();
			Objects = new GobCache();
			Party = new Party();
			Resources = new ClientResources();
			Scene = new GameScene(this);
			Screen = new GameScreen();

			Widgets = new ServerWidgetCollection();
			Widgets.Add(new ServerRootWidget(0, this, Screen.Root));
		}

		public GameActionCollection Actions { get; }

		public CharAttributeCollection Attributes { get; }

		public BuffCollection Buffs { get; }

		public Map Map { get; }

		public GobCache Objects { get; }

		public Party Party { get; }

		public ClientResources Resources { get; }

		public GameScene Scene { get; }

		public GameScreen Screen { get; }

		public GameTime Time { get; set; }

		public ServerWidgetCollection Widgets { get; }

		public Coord2D WorldPosition { get; set; }

		public void Start()
		{
			client.Connect();
		}

		public void Finish()
		{
			Dispose();
		}

		public void MessageWidget(ushort widgetId, string name, object[] args)
		{
			client.Send(new WidgetMessage(widgetId, name, args));
		}

		public void RequestMap(Coord2D gc)
		{
			var grid = Map.GetGrid(gc);
			if (grid == null && !gridRequests.Contains(gc))
			{
				gridRequests.Add(gc);
				// TODO: Queue on sender thread?
				ThreadPool.QueueUserWorkItem(o => client.Send(new MapRequestGrid(gc)));
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			client.Close();
			App.QueueOnMainThread(() => Screen.Close());
			
		}

		#region Message Handlers

		protected override void Handle(WidgetCreate args)
		{
			App.QueueOnMainThread(() =>
			{
				var parent = Widgets[args.ParentId];
				if (parent == null)
					throw new Exception($"Non-existent parent widget {args.ParentId} for {args.Id}");

				var widget = widgetFactory.Create(args.Type, args.Id, parent);
				widget.Init(args.Position, args.Args);
				Widgets.Add(widget);
			});
		}

		protected override void Handle(WidgetMessage args)
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

		protected override void Handle(WidgetDestroy args)
		{
			App.QueueOnMainThread(() => Widgets.Remove(args.WidgetId));
		}

		protected override void Handle(LoadResource args)
		{
			App.QueueOnMainThread(() => Resources.Load(args.ResourceId, args.Name));
		}

		protected override void Handle(LoadTilesets args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var binding in args.Tilesets)
					Map.BindTileset(binding);
			});
		}

		protected override void Handle(MapInvalidate args)
		{
			App.QueueOnMainThread(() => Map.InvalidateAll());
		}

		protected override void Handle(MapInvalidateGrid args)
		{
			App.QueueOnMainThread(() => RequestMap(args.Coord));
		}

		protected override void Handle(MapInvalidateRegion args)
		{
			App.QueueOnMainThread(() => Map.InvalidateRegion(args.Region));
		}

		protected override void Handle(UpdateCharAttributes args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var attr in args.Attributes)
					Attributes.AddOrUpdate(attr.Name, attr.BaseValue, attr.ModifiedValue);
			});
		}

		protected override void Handle(UpdateGameTime args)
		{
			Log.Info("UpdateTime");
		}

		protected override void Handle(UpdateAmbientLight args)
		{
			Log.Info("UpdateAmbientLight");
		}

		protected override void Handle(UpdateAstronomy astonomy)
		{
			App.QueueOnMainThread(() => Time = new GameTime(astonomy.DayTime, astonomy.MoonPhase));
		}

		protected override void Handle(UpdateActions args)
		{
			App.QueueOnMainThread(() =>
			{
				foreach (var action in args.Removed)
					Actions.Remove(action.Name);

				foreach (var action in args.Added)
					Actions.Add(action.Name);
			});
		}

		protected override void Handle(PartyChangeLeader args)
		{
			App.QueueOnMainThread(() =>
			{
				Party.LeaderId = args.LeaderId;
			});
		}

		protected override void Handle(PartyUpdate args)
		{
			App.QueueOnMainThread(() => Party.Update(args.MemberIds));
		}

		protected override void Handle(PartyUpdateMember args)
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

		protected override void Handle(UpdateGameObject args)
		{
			App.QueueOnMainThread(() =>
			{
				var updater = new GobUpdater(this);
				updater.ApplyChanges(args);
			});
		}

		protected override void Handle(MapUpdateGrid message)
		{
			App.QueueOnMainThread(() =>
			{
				gridRequests.Remove(message.Coord);
				Map.AddGrid(message);
			});
		}

		protected override void Handle(PlaySound args)
		{
			var res = Resources.Get(args.ResourceId);
			App.Audio.Play(res);
		}

		protected override void Handle(PlayMusic args)
		{
			Log.Info("PlayMusic");
		}

		//protected override void Exit()
		//{
		//	App.QueueOnMainThread(Finish);
		//}

		protected override void Handle(BuffUpdate args)
		{
			App.QueueOnMainThread(() =>
			{
				var buff = new Buff(args.Id, Resources.Get<BuffProto>(args.ResourceId));
				buff.Amount = args.AMeter;
				buff.IsMajor = args.IsMajor;
				if (!string.IsNullOrEmpty(args.Tooltip))
					buff.Tooltip = args.Tooltip;
				Buffs.AddOrUpdate(buff);
			});
		}

		protected override void Handle(BuffRemove args)
		{
			App.QueueOnMainThread(() => Buffs.Remove(args.BuffId));
		}

		protected override void Handle(BuffClearAll args)
		{
			App.QueueOnMainThread(() => Buffs.Clear());
		}

		#endregion
	}
}
