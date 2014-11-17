using System;
using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Texture : Drawable
	{
		private int id;

		public Texture(int width, int height)
		{
			this.Width = width;
			this.Height = height;

			var data = new byte[width * height * 4];

			this.id = GL.GenTexture();
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(data, PixelFormat.Rgba);
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
			this.Width = width;
			this.Height = height;

			this.id = GL.GenTexture();
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(pixels, pixelFormat);
		}

		public override void Dispose()
		{
			if (id != 0)
			{
				GL.DeleteTexture(id);
				id = 0;
			}
		}

		public int Id
		{
			get { return id; }
		}

		public TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			batch.Draw(this, x + DrawOffset.X, y + DrawOffset.Y, w, h, 0.0f, 0.0f, 1.0f, 1.0f);
		}

		public void Bind()
		{
			if (id != 0)
				GL.BindTexture(TextureTarget.Texture2D, id);
		}

		private void Upload(byte[] pixelData, PixelFormat format)
		{
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, Width, Height,
				0, format, PixelType.UnsignedByte, pixelData);
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
			this.Width = bitmap.Width;
			this.Height = bitmap.Height;

			var bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba,
				bitmapData.Width, bitmapData.Height, 0,  PixelFormat.Bgra,
				PixelType.UnsignedByte, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
		}

		private void SetFilter(TextureMinFilter min, TextureMagFilter mag)
		{
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)min);
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)mag);
		}
	}
}

