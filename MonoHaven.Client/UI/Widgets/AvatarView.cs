using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Utils;

namespace MonoHaven.UI.Widgets
{
	public class AvatarView : Widget
	{
		private static readonly Point defaultSize = new Point(74, 74);
		private static readonly Drawable missing;
		private static readonly Drawable background;
		private static readonly Drawable box;

		private readonly ISprite avatar;
		private readonly GobCache gobCache;
		private readonly int? gobId;

		static AvatarView()
		{
			missing = App.Resources.GetImage("gfx/hud/equip/missing");
			background = App.Resources.GetImage("gfx/hud/equip/bg");
			box = App.Resources.GetImage("custom/ui/wbox");
		}

		public AvatarView(Widget parent, int gobId, GobCache gobCache)
			: base(parent)
		{
			this.gobId = gobId;
			this.gobCache = gobCache;
			Resize(defaultSize.X + 10, defaultSize.Y + 10);
		}

		public AvatarView(Widget parent, IEnumerable<Delayed<ISprite>> layers)
			: base(parent)
		{
			avatar = layers != null ? new LayeredSprite(layers) : null;
			Resize(defaultSize.X + 10, defaultSize.Y + 10);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetClip(5, 5, defaultSize.X, defaultSize.Y);

			var image = gobId.HasValue
				? gobCache.Get(gobId.Value).Avatar
				: avatar;
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
