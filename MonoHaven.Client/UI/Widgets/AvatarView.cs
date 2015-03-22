using System.Drawing;
using MonoHaven.Graphics;

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
				dc.Draw(missing, 5, 5);

			dc.ResetClip();
			dc.Draw(box, 0, 0, Width, Height);
		}
	}
}
