using System.Drawing;
using OpenTK;

namespace MonoHaven.Graphics
{
	public class TextureRegion
	{
		private readonly Texture texture;
		private RectangleF bounds;
		private int width, height;

		public TextureRegion(Texture texture, int x, int y, int width, int height)
		{
			this.texture = texture;
			this.width = width;
			this.height = height;
			this.bounds = RectangleF.FromLTRB(
				(float)x / texture.Width,
				(float)y / texture.Height,
				(float)(x + width) / texture.Width,
				(float)(y + height) / texture.Height);
		}

		public RectangleF Bounds
		{
			get { return bounds; }
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		public Texture Texture
		{
			get { return texture; }
		}
	}
}
