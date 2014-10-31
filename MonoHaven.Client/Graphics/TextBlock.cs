using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SharpFont;

namespace MonoHaven.Graphics
{
	public class TextBlock : Drawable
	{
		private readonly SpriteFont font;
		private readonly StringBuilder text;
		private readonly List<TextBlockGlyph> glyphs;
		private int textWidth;

		public TextBlock(SpriteFont font)
		{
			this.font = font;
			this.text = new StringBuilder();
			this.glyphs = new List<TextBlockGlyph>();
			
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
			UpdateGlyphs(index, str.Length);
		}

		public void Insert(int index, char c)
		{
			text.Append(c);
			UpdateGlyphs(index, 1);
		}

		public void Remove(int index, int count)
		{
			text.Remove(index, count);
			glyphs.RemoveRange(index, count);
			UpdateTextWidth();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			// align text
			switch (TextAlign)
			{
				case TextAlign.Center:
					x = x + (w - textWidth) / 2;
					break;
				case TextAlign.Right:
					x = x + (w - textWidth);
					break;
			}
			// draw background
			batch.SetColor(BackgroundColor);
			batch.Draw(x, y, textWidth, font.Height);
			// draw text
			batch.SetColor(TextColor);
			foreach (var glyph in glyphs)
				glyph.Draw(batch, x, y, glyph.Width, glyph.Height);
			batch.SetColor(Color.White);
		}

		private void UpdateGlyphs(int index, int count)
		{
			int gx;
			int gy = font.Ascent;

			if (index > 0 && index < glyphs.Count)
				gx = glyphs[index - 1].Position.X;
			else if (index >= glyphs.Count)
				gx = textWidth;
			else // index == 0
				gx = 0;

			// insert new glyphs
			for (int i = 0; i < count; i++)
			{
				var glyph = font.GetGlyph(text[index + i]);
				glyphs.Insert(index + i, new TextBlockGlyph(glyph, gx, gy));
				gx += (int)glyph.Advance;
			}

			// update positions of the rest glyphs
			for (int i = index + count; i < glyphs.Count; i++)
			{
				var glyph = glyphs[i];
				glyph.Position = new Point(gx, gy);
				gx += (int)glyph.Advance;
			}

			UpdateTextWidth();
		}

		private void UpdateTextWidth()
		{
			if (glyphs.Count == 0)
				textWidth = 0;
			else
			{
				var last = glyphs[glyphs.Count - 1];
				textWidth = last.Position.X + (int)last.Advance;
			}
			
		}
	}
}
