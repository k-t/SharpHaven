using System;
using System.Collections.Generic;
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
			this.id = GL.GenTexture();
			this.Width = width;
			this.Height = height;

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

		public Texture(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			this.id = GL.GenTexture();
			Bind();
			SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
			Upload(bitmap);

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
			batch.Draw(this, x, y, w, h, 0.0f, 0.0f, 1.0f, 1.0f);
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
		
		public void Upload(Bitmap image)
		{
			this.Width = image.Width;
			this.Height = image.Height;

			var bitmapData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba,
				bitmapData.Width, bitmapData.Height, 0,  PixelFormat.Bgra,
				PixelType.UnsignedByte, bitmapData.Scan0);

			image.UnlockBits(bitmapData);
		}

		public override void Dispose()
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

