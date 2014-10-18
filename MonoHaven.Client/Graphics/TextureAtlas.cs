using System;
using System.Drawing;
using System.IO;

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

		public TextureRegion AddImage(byte[] imageData)
		{
			using (var ms = new MemoryStream(imageData))
			using (var bitmap = new Bitmap(ms))
			{
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
