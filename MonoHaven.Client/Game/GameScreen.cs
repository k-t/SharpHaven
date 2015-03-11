using System;
using C5;
using MonoHaven.Messages;
using MonoHaven.UI;
using MonoHaven.UI.Remote;
using NLog;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

		private readonly GameSession session;
		private readonly ServerWidgetFactory factory;
		private readonly IDictionary<ushort, ServerWidget> serverWidgets;

		private Container charlistContainer;
		private MapView mapView;
		private Calendar calendar;
		private MenuGrid menuGrid;
		private HudMenu hudMenu;
		private ChatWindow chatWindow;

		public GameScreen(GameSession session)
		{
			this.session = session;
			this.session.WidgetCreated += OnWidgetCreated;
			this.session.WidgetDestroyed += OnWidgetDestroyed;
			this.session.WidgetMessage += OnWidgetMessage;
			this.session.Finished += OnSessionFinished;

			factory = new ServerWidgetFactory();
			serverWidgets = new TreeDictionary<ushort, ServerWidget>();
			serverWidgets[0] = new ServerRootWidget(0, session, RootWidget);
		}

		public Action Exited;

		public void Bind(ushort id, ServerWidget widget)
		{
			serverWidgets[id] = widget;
		}

		protected override void OnClose()
		{
			session.Finish();
		}

		protected override void OnUpdate(int dt)
		{
			session.State.Objects.Tick(dt);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			if (mapView != null)
				mapView.Resize(newWidth, newHeight);

			if (charlistContainer != null)
				charlistContainer.Move(
					(newWidth - charlistContainer.Width) / 2,
					(newHeight - charlistContainer.Height) / 2);

			if (calendar != null)
				calendar.Move((Window.Width - calendar.Width) / 2, calendar.Y);

			if (menuGrid != null)
				menuGrid.Move(Window.Width - menuGrid.Width - 5, Window.Height - menuGrid.Height -5);
		
			if (hudMenu != null)
				hudMenu.Move((Window.Width - hudMenu.Width) / 2, Window.Height - hudMenu.Height + 1);

			if (chatWindow != null)
				chatWindow.Move(5, Window.Height - chatWindow.Height - 5);
		}

		private void OnWidgetCreated(WidgetCreateMessage message)
		{
			var parent = GetWidget(message.ParentId);
			if (parent == null)
				throw new Exception(string.Format(
					"Non-existent parent widget {0} for {1}",
					message.ParentId,
					message.Id));

			var swidget = factory.Create(message.Type, message.Id, parent, message.Args);
			swidget.Widget.Move(message.Location);
			HandleCreatedWidget(swidget.Widget);
			Bind(message.Id, swidget);
		}

		private void OnWidgetMessage(WidgetUpdateMessage message)
		{
			var widget = GetWidget(message.Id);
			if (widget != null)
				widget.ReceiveMessage(message.Name, message.Args);
			else
				log.Warn("UI message {1} to non-existent widget {0}",
					message.Id, message.Name);
		}

		private void OnWidgetDestroyed(ushort id)
		{
			ServerWidget widget;
			if (serverWidgets.Remove(id, out widget))
			{
				widget.Remove();
				widget.Dispose();
				foreach (var child in widget.Descendants)
				{
					serverWidgets.Remove(child.Id);
					child.Dispose();
					HandleDestroyedWidget(widget.Widget);
				}
				HandleDestroyedWidget(widget.Widget);
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", id);
		}

		private void OnSessionFinished()
		{
			App.QueueOnMainThread(() => Exited.Raise());
		}

		private ServerWidget GetWidget(ushort id)
		{
			ServerWidget widget;
			return serverWidgets.Find(ref id, out widget) ? widget : null;
		}

		private void HandleCreatedWidget(Widget widget)
		{
			if (widget is MapView)
			{
				mapView = (MapView)widget;
				mapView.Resize(Window.Size);
			}
			if (widget is Container)
			{
				charlistContainer = (Container)widget;
				charlistContainer.Move(
					(Window.Width - charlistContainer.Width) / 2,
					(Window.Height - charlistContainer.Height) / 2);
			}
			if (widget is Calendar)
			{
				calendar = (Calendar)widget;
				calendar.Move((Window.Width - calendar.Width) / 2, calendar.Y);
			}
			if (widget is MenuGrid)
			{
				menuGrid = (MenuGrid)widget;
				menuGrid.Move(Window.Width - widget.Width - 5, Window.Height - widget.Height - 5);
			}
			if (widget is Hud)
			{
				hudMenu = ((Hud)widget).Menu;
				hudMenu.Move((Window.Width - hudMenu.Width) / 2, Window.Height - hudMenu.Height + 1);
			}
			if (widget is Chat)
			{
				if (chatWindow == null)
				{
					chatWindow = new ChatWindow(RootWidget);
					chatWindow.Resize(300, 200);
					chatWindow.Move(5, Window.Height - chatWindow.Height - 5);
				}
				chatWindow.AddTab((Chat)widget);
			}
		}

		private void HandleDestroyedWidget(Widget widget)
		{
			if (widget == mapView) mapView = null;
			if (widget == charlistContainer) charlistContainer = null;
			if (widget == calendar) calendar = null;
			if (widget == menuGrid) menuGrid = null;
			if (widget == hudMenu) hudMenu = null;
		}
	}
}
