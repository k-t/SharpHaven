﻿using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class TextureRegion : Drawable
	{
		private readonly Texture texture;
		private readonly int textureX;
		private readonly int textureY;
		private RectangleF textureBounds;

		public TextureRegion(Texture texture, Rectangle region)
			: this(texture, region.X, region.Y, region.Width, region.Height)
		{}

		public TextureRegion(Texture texture, int textureX, int textureY, int width, int height)
		{
			this.texture = texture;
			this.textureX = textureX;
			this.textureY = textureY;
			this.Width = width;
			this.Height = height;
			this.textureBounds = RectangleF.FromLTRB(
				(float)textureX / texture.PotWidth,
				(float)textureY / texture.PotHeight,
				(float)(textureX + width) / texture.PotWidth,
				(float)(textureY + height) / texture.PotHeight);
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
				textureX, textureY, Width, Height,
				format, PixelType.UnsignedByte, pixels);

			return this;
		}

		public TextureRegion Upload(Bitmap image)
		{
			var bitmapData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			texture.Bind();
			GL.TexSubImage2D(texture.Target, 0,
				textureX, textureY, Width, Height,
				PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

			image.UnlockBits(bitmapData);
			return this;
		}
	}
}
