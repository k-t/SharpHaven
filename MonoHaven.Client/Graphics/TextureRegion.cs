using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

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

		public TextureRegion Upload(byte[] pixels, PixelFormat format)
		{
			texture.Bind();
			GL.TexSubImage2D(texture.Target, 0,
				x, y, Width, Height,
				format, PixelType.UnsignedByte, pixels);

			return this;
		}

		public TextureRegion Upload(Bitmap image)
		{
			var bitmapData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			texture.Bind();
			GL.TexSubImage2D(texture.Target, 0,
				x, y, Width, Height,
				PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

			image.UnlockBits(bitmapData);
			return this;
		}
	}
}
