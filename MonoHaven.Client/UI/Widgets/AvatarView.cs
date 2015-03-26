using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class AvatarView : Widget
	{
		private static readonly Point defaultSize = new Point(74, 74);
		private static readonly Drawable missing;
		private static readonly Drawable background;
		private static readonly Drawable box;

		static AvatarView()
		{
			missing = App.Resources.Get<Drawable>("gfx/hud/equip/missing");
			background = App.Resources.Get<Drawable>("gfx/hud/equip/bg");
			box = App.Resources.Get<Drawable>("custom/ui/wbox");
		}

		public AvatarView(Widget parent) : base(parent)
		{
			Resize(defaultSize.X + 10, defaultSize.Y + 10);
			BorderColor = Color.White;
		}

		// TODO: change to MouseButtonEvent
		public event Action<AvatarView, MouseButton> Click;

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

			var image = (Avatar != null ? Avatar.Image : null);
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
			Click.Raise(this, e.Button);
		}
	}
}
