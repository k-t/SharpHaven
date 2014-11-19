using System;
using System.Drawing;
using System.IO;
using OpenTK;

namespace MonoHaven.Graphics
{
	public class TextureSlice : Drawable
	{
		private readonly Texture tex;
		private readonly Vector2 uv;
		private readonly Vector2 uv2;
		private readonly bool ownsTexture;

		public TextureSlice(Texture tex, RectangleF region, bool ownsTexture = false)
		{
			this.tex = tex;
			this.ownsTexture = ownsTexture;
			this.uv = new Vector2(region.X / tex.Width, region.Y / tex.Height);
			this.uv2 = new Vector2(region.Right / tex.Width, region.Bottom / tex.Height);
			this.Width = (int)region.Width;
			this.Height = (int)region.Height;
		}

		public TextureSlice(Texture tex, int x, int y, int w, int h, bool ownsTexture = false)
			: this(tex, new RectangleF(x, y, w, h), ownsTexture)
		{
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
				throw new ArgumentNullException("bitmap");
			var tex = new Texture(bitmap.Size);
			tex.Update(bitmap);
			return new TextureSlice(tex, 0, 0, bitmap.Width, bitmap.Height, true);
		}

		public override void Dispose()
		{
			if (ownsTexture)
				tex.Dispose();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(tex, x, y, w, h, uv.X, uv.Y, uv2.X, uv2.Y);
		}

		public TextureSlice Update(Bitmap bitmap)
		{
			tex.Bind();
			tex.Update((int)(tex.Width * uv.X), (int)(tex.Height * uv.Y), bitmap);
			return this;
		}

		public TextureSlice Update(RawImage image)
		{
			tex.Bind();
			tex.Update((int)(tex.Width * uv.X), (int)(tex.Height * uv.Y), image);
			return this;
		}
	}
}
