using System;
using System.Drawing;
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

		public Hud(Widget parent, ClientState gstate) : base(parent)
		{
			menu = gstate.Screen.HudMenu;
			menu.Visible = true;

			belt = gstate.Screen.Belt;
			belt.Visible = true;

			lblError = new Label(this, Fonts.Default);
			lblError.TextColor = Color.FromArgb(192, 0, 0);
			lblError.Visible = false;

			InitMinimap(gstate);
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

		private void InitMinimap(ClientState gstate)
		{
			var window = new Window(this);
			window.Margin = 5;
			window.Move(500, 50);

			var minimap = new Minimap(window, gstate);
			minimap.Resize(250, 250);

			window.Pack();
		}
	}
}
