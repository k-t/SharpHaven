using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Texture : IDisposable
	{
		private int id;
		private readonly int width;
		private readonly int height;

		public Texture(Size size) : this(size.Width, size.Height)
		{
		}

		public Texture(int minWidth, int minHeight)
		{
			id = GL.GenTexture();
			width = MathHelper.NextPowerOfTwo(minWidth);
			height = MathHelper.NextPowerOfTwo(minHeight);
			Init();
		}

		public int Id
		{
			get { return id; }
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		public TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		public void Dispose()
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

		public void Update(Pixmap image)
		{
			Update(0, 0, image);
		}

		public void Update(int x, int y, Pixmap image)
		{
			GL.TexSubImage2D(Target, 0, x, y, image.Width, image.Height,
				image.Format, PixelType.UnsignedByte, image.PixelData);
		}

		public void Update(Bitmap bitmap)
		{
			Update(0, 0, bitmap);
		}

		public void Update(int x, int y, Bitmap bitmap)
		{
			var bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexSubImage2D(Target, 0, x, y, bitmapData.Width, bitmapData.Height,
				PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
		}

		public void Update(PixelFormat format, byte[] pixels)
		{
			GL.TexSubImage2D(Target, 0, 0, 0, Width, Height, format,
				PixelType.UnsignedByte, pixels);
		}

		private void Init()
		{
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, Width, Height,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
		}

		private void SetFilter(TextureMinFilter min, TextureMagFilter mag)
		{
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)min);
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)mag);
		}
	}
}

