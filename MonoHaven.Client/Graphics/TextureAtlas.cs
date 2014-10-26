using System;

namespace MonoHaven.Graphics
{
	public class TextureAtlas : IDisposable
	{
		private const int Padding = 2;

		private readonly Texture texture;
		private int nx;
		private int ny;
		private int maxRowHeight;

		public TextureAtlas(int width, int height)
		{
			texture = new Texture(width, height);
		}

		public TextureRegion AllocateRegion(int width, int height)
		{
			if (ny + height > texture.Height)
				// TODO: specific exceptions
				throw new Exception("Couldn't allocate region on texture.");

			if (nx + width > texture.Width)
			{
				nx = 0;
				ny += maxRowHeight + Padding;
				maxRowHeight = 0;
			}

			int x = nx;
			int y = ny;

			nx += width + Padding;

			if (height > maxRowHeight)
				maxRowHeight = height;

			return new TextureRegion(texture, x, y, width, height);
		}

		public void Dispose()
		{
			texture.Dispose();
		}
	}
}
