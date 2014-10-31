using System.Drawing;
using System.Text;

namespace MonoHaven.Graphics
{
	public class TextBlock : Drawable
	{
		private readonly SpriteFont font;
		private readonly StringBuilder text;
		private int textWidth;

		public TextBlock(SpriteFont font)
		{
			this.text = new StringBuilder();
			this.font = font;
			this.BackgroundColor = Color.Transparent;
		}

		public SpriteFont Font
		{
			get { return font; }
		}

		public StringBuilder Text
		{
			get { return text; }
		}

		public int TextWidth
		{
			get { return textWidth; }
		}

		public Color TextColor { get; set; }
		public Color BackgroundColor { get; set; }

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			if (Text.Length == 0)
			{
				textWidth = 0;
				return;
			}

			int cx = x;
			int cy = y + font.Ascent;
			textWidth = 0;

			// calculate text width
			var glyphs = new TextGlyph[Text.Length];
			for (int i = 0; i < Text.Length; i++)
			{
				glyphs[i] = font.GetGlyph(Text[i]);
				textWidth += (int)glyphs[i].Advance;
			}
			// draw background
			batch.SetColor(BackgroundColor);
			batch.Draw(x, y, textWidth, font.Height);
			// draw text
			batch.SetColor(TextColor);
			for (int i = 0; i < Text.Length; i++)
			{
				var glyph = glyphs[i];
				if (glyph.Image != null)
					glyph.Image.Draw(batch, cx + glyph.Offset.X, cy + glyph.Offset.Y, glyph.Image.Width, glyph.Image.Height);
				cx += (int)glyph.Advance;
			}
			batch.SetColor(Color.White);
		}
	}
}
