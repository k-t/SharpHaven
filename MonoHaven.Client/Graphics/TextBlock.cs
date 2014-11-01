using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

		public IReadOnlyList<TextBlockGlyph> Glyphs
		{
			get { return glyphs; }
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
			glyphs.InsertRange(index, str.Select(ConvertToGlyph));
			UpdateGlyphs(index);
		}

		public void Insert(int index, char c)
		{
			text.Insert(index, c);
			glyphs.Insert(index, ConvertToGlyph(c));
			UpdateGlyphs(index);
		}

		public void Remove(int index, int count)
		{
			text.Remove(index, count);
			glyphs.RemoveRange(index, count);
			UpdateGlyphs(index);
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

		private void UpdateGlyphs(int startIndex)
		{
			int gx;
			int gy = font.Ascent;

			if (startIndex > 0 && startIndex < glyphs.Count)
			{
				var glyph = glyphs[startIndex - 1];
				gx = glyph.Position.X + (int)glyph.Advance;
			}
			else if (startIndex >= glyphs.Count)
				gx = textWidth;
			else // index == 0
				gx = 0;

			// update glyphs positions
			for (int i = startIndex; i < glyphs.Count; i++)
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

		private TextBlockGlyph ConvertToGlyph(char c)
		{
			return new TextBlockGlyph(font.GetGlyph(c));
		}
	}
}
