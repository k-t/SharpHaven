using System;
using System.Drawing;
using System.IO;

namespace MonoHaven.Graphics
{
	public class TextureRegion : Drawable
	{
		private readonly Texture texture;
		private readonly bool isOwner;
		private readonly int x;
		private readonly int y;
		private RectangleF textureBounds;

		public TextureRegion(Texture texture, Rectangle region, bool isOwner = false)
			: this(texture, region.X, region.Y, region.Width, region.Height, isOwner)
		{
		}

		public TextureRegion(
			Texture texture,
			int x,
			int y,
			int width,
			int height,
			bool isOwner = false)
		{
			this.texture = texture;
			this.isOwner = isOwner;
			this.x = x;
			this.y = y;
			this.Width = width;
			this.Height = height;
			this.textureBounds = RectangleF.FromLTRB(
				(float)x / texture.Width,
				(float)y / texture.Height,
				(float)(x + width) / texture.Width,
				(float)(y + height) / texture.Height);
		}

		public static TextureRegion FromBitmap(byte[] bitmapData)
		{
			using (var stream = new MemoryStream(bitmapData))
			using (var bitmap = new Bitmap(stream))
				return FromBitmap(bitmap);
		}

		public static TextureRegion FromBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");
			var tex = new Texture(bitmap.Size);
			tex.Update(bitmap);
			return new TextureRegion(tex, 0, 0, bitmap.Width, bitmap.Height, true);
		}

		public override void Dispose()
		{
			if (isOwner)
				texture.Dispose();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(texture, x, y, w, h,
				textureBounds.Left, textureBounds.Top,
				textureBounds.Right, textureBounds.Bottom);
		}

		public TextureRegion Update(Bitmap bitmap)
		{
			texture.Bind();
			texture.Update(x, y, bitmap);
			return this;
		}

		public TextureRegion Update(RawImage image)
		{
			texture.Bind();
			texture.Update(x, y, image);
			return this;
		}
	}
}
