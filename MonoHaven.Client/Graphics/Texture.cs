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
		private int potHeigth;

		public Texture(int width, int height)
		{
			this.id = GL.GenTexture();
			Bind();
			SetSize(width, height);
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
		}

		public Texture(byte[] bitmapData)
		{
			if (bitmapData == null)
				throw new ArgumentNullException("bitmapData");

			this.id = GL.GenTexture();
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(bitmapData);
		}

		public Texture(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			this.id = GL.GenTexture();
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(bitmap);
		}

		public Texture(int width, int height, PixelFormat pixelFormat, byte[] pixels)
		{
			this.id = GL.GenTexture();
			Bind();
			SetSize(width, height);
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(pixels, pixelFormat);
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
			get { return potHeigth; }
		}

		public override void Dispose()
		{
			if (id != 0)
			{
				GL.DeleteTexture(id);
				id = 0;
			}
		}

		public TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(this, x, y, w, h, 0.0f, 0.0f,
				(float)Width / potWidth, (float)Height / potHeigth);
		}

		public void Bind()
		{
			if (id != 0)
				GL.BindTexture(TextureTarget.Texture2D, id);
		}

		private void Upload(byte[] pixelData, PixelFormat format)
		{
			GL.TexSubImage2D(Target, 0, 0, 0, Width, Height, format,
				PixelType.UnsignedByte, pixelData);
		}

		private void Upload(byte[] bitmapData)
		{
			using (var stream = new MemoryStream(bitmapData))
			using (var bitmap = new Bitmap(stream))
			{
				Upload(bitmap);
			}
		}
		
		private void Upload(Bitmap bitmap)
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

		private void SetFilter(TextureMinFilter min, TextureMagFilter mag)
		{
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)min);
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)mag);
		}

		private void SetSize(int width, int height)
		{
			Width = width;
			Height = height;
			potWidth = MathHelper.NextPowerOfTwo(width);
			potHeigth = MathHelper.NextPowerOfTwo(height);
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, potWidth, potHeigth,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
		}
	}
}

