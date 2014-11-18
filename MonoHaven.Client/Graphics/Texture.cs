using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Texture : Drawable
	{
		private int id;
		private int potWidth;
		private int potHeight;

		public Texture(Size size) : this(size.Width, size.Height)
		{
		}

		public Texture(int width, int height)
		{
			this.id = GL.GenTexture();
			Bind();
			SetSize(width, height);
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
		}

		public int Id
		{
			get { return id; }
		}

		public int PotWidth
		{
			get { return potWidth; }
		}

		public int PotHeight
		{
			get { return potHeight; }
		}

		public TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		public static Texture FromBitmap(byte[] bitmapData)
		{
			using (var stream = new MemoryStream(bitmapData))
			using (var bitmap = new Bitmap(stream))
				return FromBitmap(bitmap);
		}

		public static Texture FromBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");
			var tex = new Texture(bitmap.Size);
			tex.Update(bitmap);
			return tex;
		}

		public static Texture FromPixelData(
			int width,
			int height,
			PixelFormat pixelFormat,
			byte[] pixels)
		{
			var tex = new Texture(width, height);
			tex.Update(pixelFormat, pixels);
			return tex;
		}

		public override void Dispose()
		{
			if (id != 0)
			{
				GL.DeleteTexture(id);
				id = 0;
			}
		}

		public void Bind()
		{
			if (id == 0)
				throw new InvalidOperationException("Can't bind disposed texture");
			GL.BindTexture(TextureTarget.Texture2D, id);
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(this, x, y, w, h, 0.0f, 0.0f,
				(float)Width / potWidth, (float)Height / potHeight);
		}

		public void Update(PixelFormat format, byte[] pixelData)
		{
			Update(0, 0, format, pixelData);
		}

		public void Update(int x, int y, PixelFormat format, byte[] pixelData)
		{
			GL.TexSubImage2D(Target, 0, 0, 0, Width, Height, format,
				PixelType.UnsignedByte, pixelData);
		}

		public void Update(Bitmap bitmap)
		{
			Update(0, 0, bitmap);
		}

		public void Update(int x, int y, Bitmap bitmap)
		{
			SetSize(bitmap.Width, bitmap.Height);

			var bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexSubImage2D(Target, 0, 0, 0, bitmapData.Width, bitmapData.Height,
				PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
		}

		public void SetFilter(TextureMinFilter min, TextureMagFilter mag)
		{
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)min);
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)mag);
		}

		private void SetSize(int width, int height)
		{
			Width = width;
			Height = height;
			potWidth = MathHelper.NextPowerOfTwo(width);
			potHeight = MathHelper.NextPowerOfTwo(height);
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, potWidth, potHeight,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
		}
	}
}

