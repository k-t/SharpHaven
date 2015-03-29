using System;
using System.Drawing;
using System.Text;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class TextBox : Widget
	{
		private const int TextPadding = 2;
		private const int CursorBlinkRate = 500;

		private readonly StringBuilder text;
		private readonly TextLine textLine;

		private int caretIndex;
		private int caretPosition;
		private int caretOffset;
		private char? passwordChar;

		public TextBox(Widget parent) : base(parent)
		{
			text = new StringBuilder();
			textLine = new TextLine(Fonts.Default);
			textLine.TextColor = Color.Black;
			IsFocusable = true;
		}

		public string Text
		{
			get { return text.ToString(); }
			set
			{
				ClearText();
				InsertText(0, value);
				CaretIndex = text.Length;
			}
		}

		public char? PasswordChar
		{
			get { return passwordChar; }
			set
			{
				if (passwordChar == value)
					return;

				passwordChar = value;

				textLine.Clear();
				if (passwordChar.HasValue)
					textLine.Insert(0, new string(passwordChar.Value, text.Length));
				else
					textLine.Insert(0, Text);
			}
		}

		private int CaretIndex
		{
			get { return caretIndex; }
			set
			{
				caretIndex = MathHelper.Clamp(value, 0, textLine.Length);
				UpdateCaretPosition();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetClip(0, 0, Width, Height);
			dc.SetColor(Color.White);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();
			dc.Draw(textLine, TextPadding - caretOffset, TextPadding, Width, Height);
			// draw cursor
			if (IsFocused && DateTime.Now.Millisecond > CursorBlinkRate)
			{
				dc.SetColor(Color.Black);
				dc.DrawRectangle(caretPosition, TextPadding, 1, textLine.Font.Height);
				dc.ResetColor();
			}
			dc.ResetClip();
		}

		protected override void OnKeyDown(KeyEvent e)
		{
			e.Handled = true;
			switch (e.Key)
			{
				case Key.BackSpace:
					if (caretIndex > 0)
					{
						RemoveText(caretIndex - 1, 1);
						CaretIndex--;
					}
					break;
				case Key.Delete:
					if (caretIndex < text.Length)
						RemoveText(caretIndex, 1);
					break;
				case Key.Left:
					CaretIndex--;
					break;
				case Key.Right:
					CaretIndex++;
					break;
				default:
					e.Handled = false;
					base.OnKeyDown(e);
					break;
			}
		}

		protected override void OnKeyPress(KeyPressEvent e)
		{
			if (char.IsControl(e.KeyChar))
				return;
			InsertText(caretIndex, e.KeyChar);
			CaretIndex++;
			e.Handled = true;
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				var p = MapFromScreen(e.Position).Sub(TextPadding - caretOffset, TextPadding);
				var index = textLine.PointToIndex(p.X , p.Y);
				if (index != -1)
					CaretIndex = index;
			}
			e.Handled = true;
		}

		protected override void OnSizeChanged()
		{
			caretOffset = 0;
			UpdateCaretPosition();
		}

		protected override void OnDispose()
		{
			textLine.Dispose();
		}

		private void ClearText()
		{
			text.Clear();
			textLine.Clear();
		}

		private void InsertText(int index, char value)
		{
			text.Insert(index, value);
			textLine.Insert(index, passwordChar ?? value);
		}

		private void InsertText(int index, string value)
		{
			text.Insert(index, value);
			textLine.Insert(index, passwordChar.HasValue
				? new string(passwordChar.Value, text.Length)
				: value);
		}

		private void RemoveText(int index, int length)
		{
			text.Remove(index, length);
			textLine.Remove(index, length);
		}

		private int GetGlyphPosition(int glyphIndex)
		{
			int position = glyphIndex < textLine.Length
				? textLine.Glyphs[glyphIndex].Box.X
				: textLine.TextWidth;
			return position + TextPadding;
		}

		private void UpdateCaretPosition()
		{
			int glyphPosition = GetGlyphPosition(CaretIndex);
			caretPosition = MathHelper.Clamp(
				glyphPosition - caretOffset,
				TextPadding,
				Width - TextPadding * 2);
			caretOffset = glyphPosition - caretPosition;
		}
	}
}
