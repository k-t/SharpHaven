using System.Collections.Generic;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Avatar : Drawable
	{
		private readonly Texture tex;

		public Avatar(IEnumerable<Resource> layers)
		{
			var images = layers
				.Select(x => x.GetLayer<ImageData>())
				.Where(x => x != null)
				.OrderBy(x => x.Z);
			tex = new Texture(ImageUtils.Combine(images));
			Width = tex.Width;
			Height = tex.Height;
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			tex.Draw(batch, x, y, w, h);
		}

		public override void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}
	}
}
