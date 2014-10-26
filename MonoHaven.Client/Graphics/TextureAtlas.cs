using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class TextureAtlas : IDisposable
	{
		private readonly Texture texture;
		private int nx;
		private int ny;
		private int maxRowHeight;

		public TextureAtlas(int width, int height)
		{
			texture = new Texture(width, height);
		}

		public TextureRegion AddImage(
			PixelFormat pixelFormat, byte[] imageData, int width, int height)
		{
			texture.Bind();

			if (ny + height > texture.Height)
				throw new GraphicsException("Couldn't fit image into texture.");

			if (nx + width > texture.Width)
			{
				nx = 0;
				ny += maxRowHeight + 2;
				maxRowHeight = 0;
			}

			int x = nx;
			int y = ny;
			texture.Upload(x, y, width, height, pixelFormat, imageData);

			nx += width + 2;
			if (height > maxRowHeight)
				maxRowHeight = height;

			return new TextureRegion(texture, x, y, width, height);
		}

		public TextureRegion AddImage(byte[] imageData)
		{
			texture.Bind();

			using (var ms = new MemoryStream(imageData))
			using (var bitmap = new Bitmap(ms))
			{
				if (ny + bitmap.Height > texture.Height)
					throw new GraphicsException("Couldn't fit image into texture.");

				if (nx + bitmap.Width > texture.Width)
				{
					nx = 0;
					ny += maxRowHeight + 2;
					maxRowHeight = 0;
				}

				int x = nx;
				int y = ny;
				texture.Upload(x, y, bitmap.Width, bitmap.Height, bitmap);

				nx += bitmap.Width + 2;
				if (bitmap.Height > maxRowHeight)
					maxRowHeight = bitmap.Height;

				return new TextureRegion(texture, x, y, bitmap.Width, bitmap.Height);
			}
		}

		public void Dispose()
		{
			texture.Dispose();
		}
	}
}
