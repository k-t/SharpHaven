namespace MonoHaven.Graphics
{
	public class TextureRegion
	{
		private readonly Texture texture;
		private float top, bottom, left, right;
		private int width, height;

		public TextureRegion(Texture texture, int x, int y, int width, int height)
		{
			this.texture = texture;
			this.width = width;
			this.height = height;
			this.left = (float)x / texture.Width;
			this.right = (float)(x + width) / texture.Width;
			this.top = (float)y / texture.Height;
			this.bottom = (float)(y + height) / texture.Height;
		}

		public float Top
		{
			get { return top; }
		}

		public float Bottom
		{
			get { return bottom; }
		}

		public float Left
		{
			get { return left; }
		}

		public float Right
		{
			get { return right; }
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
