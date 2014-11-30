using System;
using System.Drawing;
using System.Text;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class TextBox : Widget
	{
		private const int TextPadding = 2;
		private const int CursorBlinkRate = 500;

		private readonly StringBuilder text;
		private readonly TextBlock textBlock;

		private int caretIndex;
		private int caretPosition;
		private int caretOffset;
		private char? passwordChar;

		public TextBox(Widget parent) : base(parent)
		{
			text = new StringBuilder();
			textBlock = new TextBlock(Fonts.Default);
			textBlock.TextColor = Color.Black;
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

				textBlock.Clear();
				if (passwordChar.HasValue)
					textBlock.Insert(0, new string(passwordChar.Value, text.Length));
				else
					textBlock.Insert(0, Text);
			}
		}

		private int CaretIndex
		{
			get { return caretIndex; }
			set
			{
				caretIndex = MathHelper.Clamp(value, 0, textBlock.Length);
				UpdateCaretPosition();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetClip(0, 0, Width, Height);
			dc.SetColor(Color.White);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();
			dc.Draw(textBlock, TextPadding - caretOffset, TextPadding, Width, Height);
			// draw cursor
			if (IsFocused && DateTime.Now.Millisecond > CursorBlinkRate)
			{
				dc.SetColor(Color.Black);
				dc.DrawRectangle(caretPosition, TextPadding, 1, textBlock.Font.Height);
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
				var index = textBlock.PointToIndex(p.X , p.Y);
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
			textBlock.Dispose();
		}

		private void ClearText()
		{
			text.Clear();
			textBlock.Clear();
		}

		private void InsertText(int index, char value)
		{
			text.Insert(index, value);
			textBlock.Insert(index, passwordChar ?? value);
		}

		private void InsertText(int index, string value)
		{
			text.Insert(index, value);
			textBlock.Insert(index, passwordChar.HasValue
				? new string(passwordChar.Value, text.Length)
				: value);
		}

		private void RemoveText(int index, int length)
		{
			text.Remove(index, length);
			textBlock.Remove(index, length);
		}

		private int GetGlyphPosition(int glyphIndex)
		{
			int position = glyphIndex < textBlock.Length
				? textBlock.Glyphs[glyphIndex].Box.X
				: textBlock.TextWidth;
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
