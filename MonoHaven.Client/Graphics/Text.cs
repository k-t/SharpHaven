using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MonoHaven.Graphics
{
	public class Text : Drawable
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

		public string Value
		{
			get { return value; }
			set
			{
				this.value = value;
				UpdateTexture();
			}
		}

		public override Texture GetTexture()
		{
			return texture;
		}

		public override IEnumerable<Vertex> GetVertices(Rectangle region)
		{
			return new [] {
				new Vertex(region.X, region.Y, 0, 0),
				new Vertex(region.X + Width, region.Y, 1, 0),
				new Vertex(region.X + Width, region.Y + Height, 1, 1),
				new Vertex(region.X, region.Y + Height, 0, 1)
			};
		}

		public override void Dispose()
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
			using (var bitmap = new Bitmap(size.Width + 1, size.Height + 1))
			using (var g = System.Drawing.Graphics.FromImage(bitmap))
			using (var brush = new SolidBrush(Color))
			{
				g.DrawString(value, font, brush, 0, 0, StringFormat.GenericDefault);
				texture.Bind();
				texture.Upload(bitmap);
				Width = texture.Width;
				Height = texture.Height;
			}
		}
	}
}
