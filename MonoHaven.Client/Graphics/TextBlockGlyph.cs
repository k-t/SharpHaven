using System.Drawing;

namespace MonoHaven.Graphics
{
	public class TextBlockGlyph : Drawable
	{
		private readonly Glyph glyph;
		private readonly int ascent;
		private Rectangle box;

		public TextBlockGlyph(SpriteFont font, char c)
		{
			glyph = font.GetGlyph(c);
			ascent = font.Ascent;
			Width = box.Width = (int)glyph.Advance;
			Height = box.Height = font.Height;
		}

		public Rectangle Box
		{
			get { return box; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
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
			box.Location = new Point(x, y);
		}
	}
}
