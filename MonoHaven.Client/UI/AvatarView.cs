using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Resources;

namespace MonoHaven.UI
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
			missing = App.Instance.Resources.GetTexture("gfx/hud/equip/missing");
			background = App.Instance.Resources.GetTexture("gfx/hud/equip/bg");

			using (var bitmap = EmbeddedResource.GetImage("wbox.png"))
				box = new NinePatch(TextureSlice.FromBitmap(bitmap), 8, 8, 8, 8);
		}

		public AvatarView(Widget parent, int gobId, GobCache gobCache)
			: base(parent)
		{
			this.gobId = gobId;
			this.gobCache = gobCache;
			SetSize(defaultSize.X + 10, defaultSize.Y + 10);
		}

		public AvatarView(Widget parent, IEnumerable<Delayed<ISprite>> layers)
			: base(parent)
		{
			avatar = layers != null ? new LayeredSprite(layers) : null;
			SetSize(defaultSize.X + 10, defaultSize.Y + 10);
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
