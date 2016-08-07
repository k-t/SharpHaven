using System;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class AvatarView : Widget
	{
		private static readonly Coord2d defaultSize = new Coord2d(74, 74);
		private static readonly Drawable missing;
		private static readonly Drawable background;
		private static readonly Drawable box;

		static AvatarView()
		{
			missing = App.Resources.Get<Drawable>("gfx/hud/equip/missing");
			background = App.Resources.Get<Drawable>("gfx/hud/equip/bg");
			box = App.Resources.Get<Drawable>("gfx/wbox");
		}

		public AvatarView(Widget parent) : base(parent)
		{
			BorderColor = Color.White;
			Size = new Coord2d(defaultSize.X + 10, defaultSize.Y + 10);
		}

		public event Action<AvatarView, MouseButtonEvent> Click;

		public Color BorderColor
		{
			get;
			set;
		}

		public Avatar Avatar
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetClip(5, 5, defaultSize.X, defaultSize.Y);

			var image = Avatar?.Image;
			if (image != null)
			{
				dc.Draw(background, -(background.Width - Width) / 2, -20 + 5);
				dc.Draw(image, -(background.Width - Width) / 2, -20 + 5);
			}
			else
				dc.Draw(missing, 5, 5, Width - 10, Height - 10);

			dc.ResetClip();
			dc.SetColor(BorderColor);
			dc.Draw(box, 0, 0, Width, Height);
			dc.ResetColor();
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			Click.Raise(this, e);
		}
	}
}
