using System;
using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Texture : IDisposable
	{
		private int id;
		private Size size;

		public Texture(int width, int height)
		{
			this.id = GL.GenTexture();
			this.size = new Size(width, height);

			var data = new byte[width * height * 4];

			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(PixelFormat.Rgba, data);
		}

		public Texture(byte[] imageData)
		{
			if (imageData == null)
				throw new ArgumentNullException("imageData");

			using (var ms = new MemoryStream(imageData))
			using (var image = new Bitmap(ms))
			{
				this.id = GL.GenTexture();

				Bind();
				SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
				Upload(image);
			}
		}

		public int Id
		{
			get { return id; }
		}

		public Size Size
		{
			get { return size; }
		}

		public int Width
		{
			get { return size.Width; }
		}

		public int Height
		{
			get { return size.Height; }
		}

		public TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		public void Bind()
		{
			if (id != 0)
				GL.BindTexture(TextureTarget.Texture2D, id);
		}

		public void Upload(PixelFormat format, byte[] data)
		{
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, Width, Height,
				0, format, PixelType.UnsignedByte, data);
		}

		public void Upload(int x, int y, int width, int height, PixelFormat format, byte[] data)
		{
			GL.TexSubImage2D(Target, 0, x, y, width, height, format,
				PixelType.UnsignedByte, data);
		}
		
		public void Upload(Bitmap image)
		{
			this.size = image.Size;

			var bitmapData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba,
				bitmapData.Width, bitmapData.Height, 0,  PixelFormat.Bgra,
				PixelType.UnsignedByte, bitmapData.Scan0);

			image.UnlockBits(bitmapData);
		}

		public void Upload(int x, int y, int width, int height, Bitmap image)
		{
			var bitmapData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexSubImage2D(Target, 0, x, y, width, height, PixelFormat.Bgra,
				PixelType.UnsignedByte, bitmapData.Scan0);

			image.UnlockBits(bitmapData);
		}

		public void Dispose()
		{
			if (id != 0)
			{
				GL.DeleteTexture(id);
				id = 0;
			}
		}

		private void SetFilter(TextureMinFilter min, TextureMagFilter mag)
		{
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)min);
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)mag);
		}
	}
}

