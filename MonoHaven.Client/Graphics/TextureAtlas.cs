using System;
using System.Drawing;
using System.IO;

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

		public TextureRegion Add(RawImage image)
		{
			return Allocate(image.Width, image.Height).Update(image);
		}

		public TextureRegion Add(byte[] bitmapData)
		{
			using (var stream = new MemoryStream(bitmapData))
			using (var bitmap = new Bitmap(stream))
			{
				return Add(bitmap);
			}
		}

		public TextureRegion Add(Bitmap bitmap)
		{
			return Allocate(bitmap.Width, bitmap.Height).Update(bitmap);
		}

		public TextureRegion Allocate(int width, int height)
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
