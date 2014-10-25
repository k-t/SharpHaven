using System.Collections.Generic;
using System.Drawing;

namespace MonoHaven.Graphics
{
	public class TextureRegion : Drawable
	{
		private readonly Texture texture;
		private RectangleF textureBounds;

		public TextureRegion(Texture texture, Rectangle region)
			: this(texture, region.X, region.Y, region.Width, region.Height)
		{}

		public TextureRegion(Texture texture, int x, int y, int width, int height)
		{
			this.texture = texture;
			this.Width = width;
			this.Height = height;
			this.textureBounds = RectangleF.FromLTRB(
				(float)x / texture.Width,
				(float)y / texture.Height,
				(float)(x + width) / texture.Width,
				(float)(y + height) / texture.Height);
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(texture, x, y, w, h,
				textureBounds.Left, textureBounds.Top,
				textureBounds.Right, textureBounds.Bottom);
		}
	}
}
