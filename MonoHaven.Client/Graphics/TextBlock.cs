#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace MonoHaven.Graphics
{
	public class TextBlock : Drawable
	{
		private readonly SpriteFont font;
		private readonly List<TextBlockGlyph> glyphs;
		private int textWidth;

		public TextBlock(SpriteFont font)
		{
			this.font = font;
			this.glyphs = new List<TextBlockGlyph>();
			
			BackgroundColor = Color.Transparent;
		}

		public ReadOnlyCollection<TextBlockGlyph> Glyphs
		{
			get { return glyphs.AsReadOnly(); }
		}

		public SpriteFont Font
		{
			get { return font; }
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

		public int Length
		{
			get { return glyphs.Count; }
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
			Insert(Length, str);
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
			// draw text
			batch.SetColor(TextColor);
			foreach (var glyph in glyphs)
				glyph.Draw(batch, x, y, glyph.Width, glyph.Height);
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

		private void UpdateGlyphs(int startIndex)
		{
			int gx;

			if (startIndex > 0 && startIndex <= glyphs.Count)
				gx = glyphs[startIndex - 1].Box.Right;
			else // index == 0
				gx = 0;

			// update glyphs positions
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

		private TextBlockGlyph ConvertToGlyph(char c)
		{
			return new TextBlockGlyph(font, c);
		}
	}
}
