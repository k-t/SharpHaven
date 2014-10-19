using System;
using System.Drawing;
using System.Windows.Forms;

namespace MonoHaven.Graphics
{
	public class Text : IDisposable
	{
		private readonly Font font;
		private Texture texture;
		private Color color;
		private string value;

		public Text(Font font)
			: this(font, string.Empty)
		{}

		public Text(Font font, string value)
			: this(font, value, Color.Black)
		{ }

		public Text(Font font, string value, Color color)
		{
			this.font = font;
			this.value = value;
			this.Color = color;
			this.texture = new Texture(0, 0);
		}

		public Color Color
		{
			get { return color; }
			set
			{
				this.color = value;
				UpdateTexture();
			}
		}

		public Texture Texture
		{
			get { return texture; }
		}

		public string Value
		{
			get { return value; }
			set
			{
				this.value = value;
				UpdateTexture();
			}
		}

		public void Dispose()
		{
			if (texture != null)
			{
				texture.Dispose();
				texture = null;
			}
		}

		private void UpdateTexture()
		{
			var size = TextRenderer.MeasureText(value, font);
			if (size == Size.Empty)
				return;
			using (var bitmap = new Bitmap(size.Width + 2, size.Height + 2))
			using (var g = System.Drawing.Graphics.FromImage(bitmap))
			using (var brush = new SolidBrush(Color))
			{
				g.DrawString(value, font, brush, 0, 0, StringFormat.GenericDefault);
				texture.Upload(bitmap);
			}
		}
	}
}
