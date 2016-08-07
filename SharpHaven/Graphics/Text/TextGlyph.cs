namespace SharpHaven.Graphics.Text
{
	public class TextGlyph
	{
		private readonly Glyph glyph;
		private readonly int ascent;
		private Rect box;

		public TextGlyph(SpriteFont font, char c)
		{
			glyph = font.GetGlyph(c);
			ascent = font.Ascent;
			box.Size = new Coord2d((int)glyph.Advance, font.Height);
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
			if (glyph.Image == null)
				return;

			glyph.Image.Draw(batch,
				x + box.X + glyph.Offset.X,
				y + box.Y + glyph.Offset.Y + ascent,
				glyph.Image.Width,
				glyph.Image.Height);
		}

		public void SetPosition(int x, int y)
		{
			box.Location = new Coord2d(x, y);
		}
	}
}
