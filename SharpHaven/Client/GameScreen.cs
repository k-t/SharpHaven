using System;
using OpenTK.Input;
using SharpHaven.Input;
using SharpHaven.UI;
using SharpHaven.UI.Widgets;

namespace SharpHaven.Client
{
	public class GameScreen : BaseScreen
	{
		private readonly Container container;
		private readonly MapView mapView;
		private readonly Calendar calendar;
		private readonly MenuGrid menuGrid;
		private readonly HudMenu hudMenu;
		private readonly Belt belt;
		private readonly ChatWindow chatWindow;
		private readonly EscapeWindow escapeWindow;
		private readonly CombatMeter combatMeter;
		private readonly CombatView combatView;
		private readonly CombatWindow combatWindow;

		public GameScreen()
		{
			escapeWindow = new EscapeWindow(RootWidget);
			escapeWindow.Visible = false;
			escapeWindow.Closed += () => escapeWindow.Visible = false;
			escapeWindow.Logout += Close;
			escapeWindow.Exit += App.Exit;

			container = new Container(Root);
			// HACK: to display character selection screen nicely
			container.Resize(800, 600);
			container.Visible = false;

			mapView = new MapView(Root);
			mapView.Visible = false;

			calendar = new Calendar(Root);
			calendar.Visible = false;

			menuGrid = new MenuGrid(Root);
			menuGrid.Visible = false;

			hudMenu = new HudMenu(Root);
			hudMenu.Visible = false;

			belt = new Belt(Root);
			belt.Visible = false;

			chatWindow = new ChatWindow(Root);
			chatWindow.Resize(300, 200);
			chatWindow.Visible = false;

			combatMeter = new CombatMeter(Root);
			combatMeter.Visible = false;

			combatView = new CombatView(Root);
			combatView.Visible = false;

			combatWindow = new CombatWindow(Root);
			combatWindow.Move(100, 100);
			combatWindow.Visible = false;

			RootWidget.KeyDown += OnKeyDown;
		}

		public event Action Closed;

		public Widget Root
		{
			get { return RootWidget; }
		}

		public Container Container
		{
			get { return container; }
		}

		public CombatMeter CombatMeter
		{
			get { return combatMeter; }
		}

		public CombatView CombatView
		{
			get { return combatView; }
		}

		public CombatWindow CombatWindow
		{
			get { return combatWindow; }
		}

		public Belt Belt
		{
			get { return belt; }
		}

		public Calendar Calendar
		{
			get { return calendar; }
		}

		public ChatWindow Chat
		{
			get { return chatWindow; }
		}

		public HudMenu HudMenu
		{
			get { return hudMenu; }
		}

		public MapView MapView
		{
			get { return mapView; }
		}

		public MenuGrid MenuGrid
		{
			get { return menuGrid; }
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
			mapView.Resize(newWidth, newHeight);
			belt.Move((newWidth - belt.Width) / 2, newHeight - belt.Height * 2 - 10);
			calendar.Move((newWidth - calendar.Width) / 2, calendar.Y);
			container.Move((newWidth - container.Width) / 2, (newHeight - container.Height) / 2);
			chatWindow.Move(5, newHeight - chatWindow.Height - 5);
			menuGrid.Move(newWidth - menuGrid.Width - 5, newHeight - menuGrid.Height - 5);
			hudMenu.Move((newWidth - hudMenu.Width) / 2, newHeight - hudMenu.Height - 5);
			escapeWindow.Move((newWidth - 100) / 2, (newHeight - 100) / 2);
			combatMeter.Move((newWidth - calendar.Width) / 2, calendar.Y);
			combatView.Move((newWidth - combatView.Width - 10), 10);
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
	}
}
