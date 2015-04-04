using System;
using MonoHaven.Input;
using MonoHaven.UI;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private Container charlistContainer;
		private MapView mapView;
		private Calendar calendar;
		private MenuGrid menuGrid;
		private HudMenu hudMenu;
		private Belt belt;
		private ChatWindow chatWindow;
		private EscapeWindow escapeWindow;

		public GameScreen()
		{
			escapeWindow = new EscapeWindow(RootWidget);
			escapeWindow.Visible = false;
			escapeWindow.Closed += () => escapeWindow.Visible = false;
			escapeWindow.Logout += Close;
			escapeWindow.Exit += App.Exit;

			RootWidget.KeyDown += OnKeyDown;
		}

		public Action Closed;

		public Widget Root
		{
			get { return RootWidget; }
		}

		public void Close()
		{
			Closed.Raise();
		}

		protected override void OnClose()
		{
			Closed.Raise();
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
				hudMenu.Move((Window.Width - hudMenu.Width) / 2, Window.Height - hudMenu.Height - 5);

			if (belt != null)
				belt.Move((Window.Width - belt.Width) / 2, Window.Height - belt.Height * 2 - 10);

			if (chatWindow != null)
				chatWindow.Move(5, Window.Height - chatWindow.Height - 5);

			escapeWindow.Move((Window.Width - 100) / 2, (Window.Height - 100) / 2);
		}

		private void OnKeyDown(KeyEvent e)
		{
			if (e.Key == Key.Escape)
			{
				escapeWindow.Visible = !escapeWindow.Visible;

				// bring to front
				escapeWindow.Remove();
				RootWidget.AddChild(escapeWindow);

				e.Handled = true;
			}
		}

		public void HandleCreatedWidget(Widget widget)
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
				hudMenu.Move((Window.Width - hudMenu.Width) / 2, Window.Height - hudMenu.Height - 5);

				belt = ((Hud)widget).Belt;
				belt.Move((Window.Width - belt.Width) / 2, Window.Height - belt.Height * 2 - 10);
			}
			if (widget is Chat)
			{
				if (chatWindow == null)
				{
					chatWindow = new ChatWindow(RootWidget);
					chatWindow.Resize(300, 200);
					chatWindow.Move(5, Window.Height - chatWindow.Height - 5);
				}
				chatWindow.AddChat((Chat)widget);
			}
		}

		public void HandleDestroyedWidget(Widget widget)
		{
			if (mouseFocus == widget) mouseFocus = null;
			if (keyboardFocus == widget) keyboardFocus = null;

			if (widget == mapView) mapView = null;
			if (widget == charlistContainer) charlistContainer = null;
			if (widget == calendar) calendar = null;
			if (widget == menuGrid) menuGrid = null;
			if (widget == hudMenu) hudMenu = null;

			if (widget is Chat)
			{
				if (chatWindow != null)
					chatWindow.RemoveChat((Chat)widget);
			}
		}
	}
}
