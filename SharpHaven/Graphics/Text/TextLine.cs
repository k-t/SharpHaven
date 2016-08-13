using System.Collections.Generic;
using System.Linq;
using Haven;

namespace SharpHaven.Graphics.Text
{
	public class TextLine : Drawable
	{
		private readonly SpriteFont font;
		private readonly List<TextGlyph> glyphs;
		private int textWidth;

		public TextLine(SpriteFont font)
		{
			this.font = font;
			this.glyphs = new List<TextGlyph>();
			this.size.Y = font.Height;

			BackgroundColor = Color.Transparent;
			OutlineColor = Color.Transparent;
		}

		public IList<TextGlyph> Glyphs
		{
			get { return glyphs; }
		}

		public SpriteFont Font
		{
			get { return font; }
		}

		public TextAlign TextAlign { get; set; }

		public int TextWidth
		{
			get { return textWidth; }
		}

		public int Length
		{
			get { return glyphs.Count; }
		}

		public Color TextColor { get; set; }

		public Color BackgroundColor { get; set; }

		public Color OutlineColor { get; set; }

		public void Append(string str)
		{
			Insert(Length, str);
		}

		public void AppendFormat(string format, params object[] args)
		{
			Append(string.Format(format, args));
		}

		public void Append(char c)
		{
			Insert(Length, c);
		}

		public void Clear()
		{
			glyphs.Clear();
			UpdateTextWidth();
		}

		public void Insert(int index, string str)
		{
			glyphs.InsertRange(index, str.Select(ConvertToGlyph));
			UpdateGlyphs(index);
		}

		public void Insert(int index, char c)
		{
			glyphs.Insert(index, ConvertToGlyph(c));
			UpdateGlyphs(index);
		}

		public void Remove(int index, int count)
		{
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
			// draw outline
			if (OutlineColor != Color.Transparent)
			{
				batch.SetColor(OutlineColor);
				foreach (var glyph in glyphs)
					glyph.DrawOutline(batch, x, y);
			}
			// draw text
			batch.SetColor(TextColor);
			foreach (var glyph in glyphs)
				glyph.Draw(batch, x, y);
			batch.SetColor(Color.White);
		}

		public int PointToIndex(int x, int y)
		{
			if (x <= 0) return 0;
			if (x > textWidth) return Length;
			for (int i = 0; i < glyphs.Count; i++)
				if (glyphs[i].Box.Contains(x, y))
					return i;
			return -1;
		}

		public void SetWidth(int width)
		{
			size.X = width;
		}

		private void UpdateGlyphs(int startIndex)
		{
			int gx;

			if (startIndex > 0 && startIndex <= glyphs.Count)
				gx = glyphs[startIndex - 1].Box.Right;
			else // index == 0
				gx = 0;

			// update glyph positions
			for (int i = startIndex; i < glyphs.Count; i++)
			{
				glyphs[i].SetPosition(gx, 0);
				gx = glyphs[i].Box.Right;
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
				textWidth = last.Box.Right;
			}
		}

		private TextGlyph ConvertToGlyph(char c)
		{
			return new TextGlyph(font, c);
		}
	}
}