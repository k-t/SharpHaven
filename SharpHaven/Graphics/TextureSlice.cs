using System;
using System.Drawing;
using System.IO;
using Haven;
using OpenTK;

namespace SharpHaven.Graphics
{
	public class TextureSlice : IDisposable
	{
		private readonly Texture tex;
		private readonly Vector2 uv;
		private readonly Vector2 uv2;
		private readonly bool ownsTexture;

		public TextureSlice(Texture tex, RectF region, bool ownsTexture = false)
		{
			this.tex = tex;
			this.ownsTexture = ownsTexture;
			this.uv = new Vector2(region.X / tex.Width, region.Y / tex.Height);
			this.uv2 = new Vector2(region.Right / tex.Width, region.Bottom / tex.Height);
		}

		public TextureSlice(Texture tex, int x, int y, int w, int h, bool ownsTexture = false)
			: this(tex, new RectF(x, y, w, h), ownsTexture)
		{
		}

		private int X
		{
			get { return (int)(tex.Width * uv.X); }
		}

		private int Y
		{
			get { return (int)(tex.Height * uv.Y); }
		}

		public int Width
		{
			get { return (int)(tex.Width * (uv2.X - uv.X)); }
		}

		public int Height
		{
			get { return (int)(tex.Height * (uv2.Y - uv.Y)); }
		}

		public static TextureSlice FromBitmap(byte[] bitmapData)
		{
			using (var stream = new MemoryStream(bitmapData))
			using (var bitmap = new Bitmap(stream))
				return FromBitmap(bitmap);
		}

		public static TextureSlice FromBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));
			var tex = new Texture(bitmap.Size);
			tex.Update(bitmap);
			return new TextureSlice(tex, 0, 0, bitmap.Width, bitmap.Height, true);
		}

		public void Dispose()
		{
			if (ownsTexture)
				tex.Dispose();
		}

		public void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(tex, x, y, w, h, uv.X, uv.Y, uv2.X, uv2.Y);
		}

		public TextureSlice Update(Bitmap bitmap)
		{
			tex.Bind();
			tex.Update(X, Y, bitmap);
			return this;
		}

		public TextureSlice Update(Pixmap image)
		{
			tex.Bind();
			tex.Update(X, Y, image);
			return this;
		}

		public TextureSlice Slice(Rect region)
		{
			int offsetX = X;
			int offsetY = Y;
			if (offsetX + region.Width > Width || offsetY + region.Height > Height)
				throw new ArgumentException("Slice is out bounds");
			return new TextureSlice(tex, region);
		}

		public TextureSlice Slice(int x, int y, int width, int height)
		{
			return Slice(new Rect(x, y, width, height));
		}
	}
}
