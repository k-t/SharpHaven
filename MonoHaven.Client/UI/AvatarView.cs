using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class AvatarView : Widget
	{
		private static readonly Point defaultSize = new Point(74, 74);
		private static readonly Texture missing;

		private readonly Avatar avatar;

		static AvatarView()
		{
			missing = App.Instance.Resources.GetTexture("gfx/hud/equip/missing");
		}

		public AvatarView(Widget parent, IEnumerable<Resource> resources)
			: base(parent)
		{
			avatar = new Avatar(resources);
			SetSize(defaultSize.X, defaultSize.Y);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			//dc.Draw(missing, 0, 0);
			dc.SetClip(0, 0, Width, Height);
			dc.Draw(avatar, -70, -20);
			dc.ResetClip();
		}

		protected override void OnDispose()
		{
			avatar.Dispose();
		}
	}
}
