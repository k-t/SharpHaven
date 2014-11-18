using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class AvatarView : Widget
	{
		private static readonly Point defaultSize = new Point(74, 74);
		private static readonly Drawable missing;
		private static readonly Drawable background;
		private static readonly Drawable box;

		private readonly Avatar avatar;

		static AvatarView()
		{
			missing = App.Instance.Resources.GetTexture("gfx/hud/equip/missing");
			background = App.Instance.Resources.GetTexture("gfx/hud/equip/bg");

			using (var bitmap = EmbeddedResource.GetImage("wbox.png"))
				box = new NinePatch(bitmap, 8, 8, 8, 8);
		}

		public AvatarView(Widget parent) : this(parent, null)
		{
		}

		public AvatarView(Widget parent, IEnumerable<Delayed<Resource>> resources)
			: base(parent)
		{
			avatar = resources != null ? new Avatar(resources) : null;
			SetSize(defaultSize.X + 10, defaultSize.Y + 10);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetClip(5, 5, defaultSize.X, defaultSize.Y);
			if (avatar != null)
			{
				dc.Draw(background, -(background.Width - Width) / 2, -20 + 5);
				dc.Draw(avatar, -(background.Width - Width) / 2, -20 + 5);
			}
			else
				dc.Draw(missing, 5, 5);
			dc.ResetClip();
			dc.Draw(box, 0, 0, Width, Height);
		}

		protected override void OnDispose()
		{
			avatar.Dispose();
		}
	}
}
