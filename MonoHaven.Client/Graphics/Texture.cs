using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MonoHaven.Resources;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Texture : IDisposable
	{
		private bool _disposed;
		private readonly int _id;
		private readonly Size _sz;

		private Texture(int id, Size sz)
		{
			_disposed = false;
			_id = id;
			_sz = sz;
		}

		public int Id
		{
			get { return _id; }
		}

		public Size Size
		{
			get { return _sz; }
		}

		public int Width
		{
			get { return _sz.Width; }
		}

		public int Height
		{
			get { return _sz.Height; }
		}

		public static Texture FromImage(ImageLayer imageData)
		{
			if (imageData == null)
				throw new ArgumentNullException("imageData");

			using (var ms = new MemoryStream(imageData.Data))
			using (var image = new Bitmap(ms))
			{
				int id = CreateTexture();

				var bitmapData = image.LockBits(
					new Rectangle(0, 0, image.Width, image.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, id);
				GL.TexImage2D(
					TextureTarget.Texture2D,
					0,
					PixelInternalFormat.Rgba,
					bitmapData.Width, bitmapData.Height,
					0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte,
					bitmapData.Scan0);
				GL.TexParameter(
					TextureTarget.Texture2D,
					TextureParameterName.TextureMinFilter,
					(int)TextureMinFilter.Nearest);
				GL.TexParameter(
					TextureTarget.Texture2D,
					TextureParameterName.TextureMagFilter,
					(int)TextureMagFilter.Nearest);

				image.UnlockBits(bitmapData);

				return new Texture(id, new Size(image.Width, image.Height));
			}
		}

		private static int CreateTexture()
		{
			int tex;
			GL.GenTextures(1, out tex);
			GL.BindTexture(TextureTarget.Texture2D, tex);
			return tex;
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			GL.DeleteTexture(_id);

			_disposed = true;
		}
	}
}

