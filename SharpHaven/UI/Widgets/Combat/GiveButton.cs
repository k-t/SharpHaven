using System;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class GiveButton : Widget
	{
		private static readonly Drawable bg;
		private static readonly Drawable ol;
		private static readonly Drawable or;
		private static readonly Drawable sl;
		private static readonly Drawable sr;

		static GiveButton()
		{
			bg = App.Resources.Get<Drawable>("gfx/hud/combat/knapp/knapp");
			ol = App.Resources.Get<Drawable>("gfx/hud/combat/knapp/ol");
			or = App.Resources.Get<Drawable>("gfx/hud/combat/knapp/or");
			sl = App.Resources.Get<Drawable>("gfx/hud/combat/knapp/sl");
			sr = App.Resources.Get<Drawable>("gfx/hud/combat/knapp/sr");
		}

		public GiveButton(Widget parent) : base(parent)
		{
			Size = bg.Size;
		}

		public event Action<MouseButtonEvent> Click;

		public int State
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (State == 0)
				dc.SetColor(255, 192, 192, 255);
			else if (State == 1)
				dc.SetColor(192, 192, 255, 255);
			else if (State == 2)
				dc.SetColor(192, 255, 192, 255);

			dc.Draw(bg, 0, 0, Width, Height);
			dc.ResetColor();

			dc.Draw((State & 1) != 0 ? ol : sl, 0, 0, Width, Height);
			dc.Draw((State & 2) != 0 ? or : sr, 0, 0, Width, Height);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			Click.Raise(e);
		}
	}
}
