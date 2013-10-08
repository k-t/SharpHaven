using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven
{
	public class Tex : IDisposable
	{
		private bool _disposed;
		private readonly int _id;
		private readonly Size _sz;

		private Tex(int id, Size sz)
		{
			_disposed = false;
			_id = id;
			_sz = sz;
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

		public void Render(int x, int y, int width, int height)
		{
			GL.Color4(255f, 255f, 255f, 255f);

			GL.BindTexture(TextureTarget.Texture2D, _id);

			GL.Begin(BeginMode.Quads);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y, 0);

			GL.TexCoord2(1, 0);
			GL.Vertex3(x + width, y, 0);

			GL.TexCoord2(1, 1);
			GL.Vertex3(x + width, y + height, 0);

			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y + height, 0);

			GL.End();
		}

		public static Tex FromImage(MonoHaven.Resources.Image img)
		{
			if (img == null)
				throw new ArgumentNullException("img");

			using (var ms = new MemoryStream(img.Data))
			using (var image = new Bitmap(ms))
			{
				int id = CreateTexture();

				var imageData = image.LockBits(
					new Rectangle(0, 0, image.Width, image.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, id);
				GL.TexImage2D(
					TextureTarget.Texture2D,
					0,
					PixelInternalFormat.Rgba,
					imageData.Width, imageData.Height,
					0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte,
					imageData.Scan0);
				GL.TexParameter(
					TextureTarget.Texture2D,
					TextureParameterName.TextureMinFilter,
					(int)TextureMinFilter.Nearest);
				GL.TexParameter(
					TextureTarget.Texture2D,
					TextureParameterName.TextureMagFilter,
					(int)TextureMagFilter.Nearest);

				image.UnlockBits(imageData);

				return new Tex(id, new Size(image.Width, image.Height));
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

