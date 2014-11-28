using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class Pixmap
	{
		private readonly PixelFormat format;
		private readonly byte[] pixelData;
		private readonly Size size;

		public Pixmap(PixelFormat format, Size size)
		{
			this.size = size;
			this.format = format;
			int bytesPerPixel = GetPixelSize(format);
			this.pixelData = new byte[size.Width * size.Height * bytesPerPixel];
		}

		public Pixmap(PixelFormat format, int width, int height)
			: this(format, new Size(width, height))
		{
		}

		public PixelFormat Format
		{
			get { return format; }
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

		public byte[] PixelData
		{
			get { return pixelData; }
		}

		private static int GetPixelSize(PixelFormat format)
		{
			switch (format)
			{
				case PixelFormat.Bgra:
				case PixelFormat.Rgba:
					return 4;
				default:
					throw new ArgumentException("Unsupported image format");
			}
		}
	}
}
