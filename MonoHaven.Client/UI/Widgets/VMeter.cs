using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class VMeter : Widget
	{
		private static readonly Drawable background;
		private static readonly Drawable foreground;

		static VMeter()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/vm-frame");
			foreground = App.Resources.Get<Drawable>("gfx/hud/vm-tex");
		}

		public VMeter(Widget parent) : base(parent)
		{
			Color = Color.White;
			Resize(background.Size);
		}

		public int Amount
		{
			get;
			set;
		}

		public Color Color
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
			dc.SetColor(Color);
			int h = Height - 6;
			h = (h * Amount) / 100;
			dc.SetClip(0, Height - 3 - h, Width, h);
			dc.Draw(foreground, 0, 0);
			dc.ResetClip();
			dc.ResetColor();
		}
	}
}
