using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MonoHaven.Graphics
{
	public class TextBlock : Drawable
	{
		private readonly SpriteFont font;
		private readonly StringBuilder text;
		private readonly List<TextGlyph> glyphs;
		private int textWidth;

		public TextBlock(SpriteFont font)
		{
			this.font = font;
			this.text = new StringBuilder();
			this.glyphs = new List<TextGlyph>();
			
			BackgroundColor = Color.Transparent;
		}

		public SpriteFont Font
		{
			get { return font; }
		}

		public string Text
		{
			get { return text.ToString(); }
			set
			{
				Clear();
				Append(value);
			}
		}

		public TextAlign TextAlign
		{
			get;
			set;
		}

		public int TextWidth
		{
			get { return textWidth; }
		}

		public int TextLength
		{
			get { return text.Length; }
		}

		public Color TextColor
		{
			get;
			set;
		}

		public Color BackgroundColor
		{
			get;
			set;
		}

		public void Append(string str)
		{
			Insert(TextLength, str);
		}

		public void Append(char c)
		{
			Insert(TextLength, c);
		}

		public void Clear()
		{
			text.Clear();
			glyphs.Clear();
			UpdateTextWidth();
		}

		public void Insert(int index, string str)
		{
			text.Insert(index, str);
			InsertGlyphs(index, str.Length);
		}

		public void Insert(int index, char c)
		{
			text.Append(c);
			InsertGlyphs(index, 1);
		}

		public void Remove(int index, int count)
		{
			text.Remove(index, count);
			glyphs.RemoveRange(index, count);
			UpdateTextWidth();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			int cx = x;
			int cy = y + font.Ascent;

			// align text
			switch (TextAlign)
			{
				case TextAlign.Center:
					cx = x + (w - textWidth) / 2;
					break;
				case TextAlign.Right:
					cx = x + (w - textWidth);
					break;
			}
			// draw background
			batch.SetColor(BackgroundColor);
			batch.Draw(cx, y, textWidth, font.Height);
			// draw text
			batch.SetColor(TextColor);
			foreach (var glyph in glyphs)
			{
				if (glyph.Image != null)
					glyph.Image.Draw(batch, cx + glyph.Offset.X, cy + glyph.Offset.Y, glyph.Image.Width, glyph.Image.Height);
				cx += (int)glyph.Advance;
			}
			batch.SetColor(Color.White);
		}

		private void InsertGlyphs(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				var glyph = font.GetGlyph(text[index + i]);
				glyphs.Insert(index + i, glyph);
			}
			UpdateTextWidth();
		}

		private void UpdateTextWidth()
		{
			textWidth = 0;
			foreach (var glyph in glyphs)
				textWidth += (int)glyph.Advance;
		}
	}
}
