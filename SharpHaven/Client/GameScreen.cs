using System;
using Haven;
using OpenTK.Input;
using SharpHaven.Graphics.Sprites.Fx;
using SharpHaven.Input;
using SharpHaven.UI;
using SharpHaven.UI.Widgets;

namespace SharpHaven.Client
{
	public class GameScreen : BaseScreen
	{
		private readonly EscapeWindow escapeWindow;

		public GameScreen()
		{
			escapeWindow = new EscapeWindow(RootWidget);
			escapeWindow.Visible = false;
			escapeWindow.Closed += () => escapeWindow.Visible = false;
			escapeWindow.Logout += Close;
			escapeWindow.Exit += App.Exit;

			Container = new Container(Root);
			// HACK: to display character selection screen nicely
			Container.Resize(800, 600);
			Container.Visible = false;

			MapView = new MapView(Root);
			MapView.Visible = false;

			Calendar = new Calendar(Root);
			Calendar.Visible = false;

			MenuGrid = new MenuGrid(Root);
			MenuGrid.Visible = false;

			HudMenu = new HudMenu(Root);
			HudMenu.Visible = false;

			Belt = new Belt(Root);
			Belt.Visible = false;

			Chat = new ChatWindow(Root);
			Chat.Resize(300, 200);
			Chat.Visible = false;

			CombatMeter = new CombatMeter(Root);
			CombatMeter.Visible = false;

			CombatView = new CombatView(Root);
			CombatView.Visible = false;

			CombatWindow = new CombatWindow(Root);
			CombatWindow.Move(100, 100);
			CombatWindow.Visible = false;

			Aim = new AimWidget(Root);
			Aim.Visible = false;

			RegisterHotkeys();
		}

		public event Action Closed;

		public Widget Root
		{
			get { return RootWidget; }
		}

		public AimWidget Aim { get; }

		public Container Container { get; }

		public CombatMeter CombatMeter { get; }

		public CombatView CombatView { get; }

		public CombatWindow CombatWindow { get; }

		public Belt Belt { get; }

		public Calendar Calendar { get; }

		public ChatWindow Chat { get; }

		public HudMenu HudMenu { get; }

		public MapView MapView { get; }

		public MenuGrid MenuGrid { get; }

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
			MapView.Resize(newWidth, newHeight);
			Belt.Move((newWidth - Belt.Width) / 2, newHeight - Belt.Height * 2 - 10);
			Calendar.Move((newWidth - Calendar.Width) / 2, Calendar.Y);
			Container.Move((newWidth - Container.Width) / 2, (newHeight - Container.Height) / 2);
			Chat.Move(5, newHeight - Chat.Height - 5);
			MenuGrid.Move(newWidth - MenuGrid.Width - 5, newHeight - MenuGrid.Height - 5);
			HudMenu.Move((newWidth - HudMenu.Width) / 2, newHeight - HudMenu.Height - 5);
			escapeWindow.Move((newWidth - 100) / 2, (newHeight - 100) / 2);
			CombatMeter.Move((newWidth - Calendar.Width) / 2, Calendar.Y);
			CombatView.Move((newWidth - CombatView.Width - 10), 10);
			Aim.Move((newWidth - Aim.Width) / 2, (newHeight - Aim.Height) / 2);
		}

		private void RegisterHotkeys()
		{
			// General
			Hotkeys.Register(Key.Escape, () =>
			{
				escapeWindow.Visible = !escapeWindow.Visible;
				// bring to front
				escapeWindow.Remove();
				RootWidget.AddChild(escapeWindow);
			});

			// Map View
			Hotkeys.Register(Key.Up, () => MapView.MoveCamera(0, -50));
			Hotkeys.Register(Key.Down, () => MapView.MoveCamera(0, 50));
			Hotkeys.Register(Key.Left, () => MapView.MoveCamera(-50, 0));
			Hotkeys.Register(Key.Right, () => MapView.MoveCamera(50, 0));
			Hotkeys.Register(Key.Home, MapView.CenterCamera);
			Hotkeys.Register(Key.Keypad7, MapView.CenterCamera);
		}
	}
}
