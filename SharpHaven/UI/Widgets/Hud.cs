using System;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class Hud : Widget
	{
		private readonly Belt belt;
		private readonly HudMenu menu;
		private readonly Label lblError;
		private DateTime errorTime;

		public Hud(Widget parent, ClientSession session) : base(parent)
		{
			menu = session.Screen.HudMenu;
			menu.Visible = true;

			belt = session.Screen.Belt;
			belt.Visible = true;

			lblError = new Label(this, Fonts.Default);
			lblError.TextColor = Color.FromArgb(192, 0, 0);
			lblError.Visible = false;

			InitMinimap(session);
		}

		public HudMenu Menu
		{
			get { return menu; }
		}

		public Belt Belt
		{
			get { return belt; }
		}

		public void ShowError(string error)
		{
			lblError.Text = error;
			lblError.Visible = true;
			lblError.Move(10, 100);
			errorTime = DateTime.Now.AddSeconds(3);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (errorTime < DateTime.Now)
				lblError.Visible = false;
		}

		protected override void OnDispose()
		{
			menu.Visible = false;
			belt.Visible = false;
		}

		private void InitMinimap(ClientSession session)
		{
			var window = new Window(this);
			window.Margin = 5;
			window.Move(500, 50);

			var minimap = new Minimap(window, session);
			minimap.Resize(250, 250);

			window.Pack();
		}
	}
}
