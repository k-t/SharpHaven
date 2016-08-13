using Haven;

namespace SharpHaven.Graphics.Text
{
	public class TextGlyph
	{
		private readonly SpriteFont font;
		private readonly char c;
		private readonly Glyph glyph;
		private readonly int ascent;
		private Glyph outline;
		private Rect box;

		public TextGlyph(SpriteFont font, char c)
		{
			this.font = font;
			this.c = c;
			this.glyph = font.GetGlyph(c);
			this.ascent = font.Ascent;
			this.box.Size = new Point2D((int)glyph.Advance, font.Height);
		}

		public Rect Box
		{
			get { return box; }
		}

		public int Width
		{
			get { return box.Width; }
		}

		public int Height
		{
			get { return box.Height; }
		}

		public void Draw(SpriteBatch batch, int x, int y)
		{
			glyph.Image?.Draw(batch,
				x + box.X + glyph.Offset.X,
				y + box.Y + glyph.Offset.Y + ascent,
				glyph.Image.Width,
				glyph.Image.Height);
		}

		public void DrawOutline(SpriteBatch batch, int x, int y)
		{
			if (outline == null)
				outline = font.GetGlyphOutline(c);

			outline.Image?.Draw(batch,
				x + box.X + outline.Offset.X,
				y + box.Y + outline.Offset.Y + ascent,
				outline.Image.Width,
				outline.Image.Height);
		}

		public void SetPosition(int x, int y)
		{
			box.Location = new Point2D(x, y);
		}
	}
}
