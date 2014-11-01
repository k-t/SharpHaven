using System;
using System.Drawing;
using System.Text;
using MonoHaven.Graphics;
using MonoHaven.Resources;
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
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		private int caretPosition;
		private char? passwordChar;

		public TextBox(Widget parent)
			: base(parent)
		{
			text = new StringBuilder();
			textBlock = new TextBlock(Fonts.Default);
			textBlock.TextColor = Color.Black;
			borderTexture = new Texture(EmbeddedResource.GetImage("textbox.png"));
			border = new NinePatch(borderTexture, 2, 2, 2, 2);

			IsFocusable = true;
		}

		public string Text
		{
			get { return text.ToString(); }
			set
			{
				ClearText();
				InsertText(0, value);
				caretPosition = text.Length;
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

		private int CaretPosition
		{
			get { return caretPosition; }
			set { caretPosition = MathHelper.Clamp(value, 0, textBlock.Length); }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(border, 0, 0, Width, Height);
			dc.Draw(textBlock, TextPadding, TextPadding, Width, Height);

			// draw cursor
			if (IsFocused && DateTime.Now.Millisecond > CursorBlinkRate)
			{
				int cx = caretPosition < textBlock.Length
					? textBlock.Glyphs[caretPosition].Box.X
					: textBlock.TextWidth;

				dc.SetColor(Color.Black);
				dc.DrawRectangle(TextPadding + cx, TextPadding, 1, textBlock.Font.Height);
				dc.ResetColor();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.Handled = true;
			switch (e.Key)
			{
				case Key.BackSpace:
					if (caretPosition > 0)
					{
						RemoveText(caretPosition - 1, 1);
						CaretPosition--;
					}
					break;
				case Key.Delete:
					if (caretPosition < text.Length)
						RemoveText(caretPosition, 1);
					break;
				case Key.Left:
					CaretPosition--;
					break;
				case Key.Right:
					CaretPosition++;
					break;
				default:
					e.Handled = false;
					break;
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (char.IsControl(e.KeyChar))
				return;

			InsertText(caretPosition, e.KeyChar);
			CaretPosition++;
			e.Handled = true;
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				var p = PointToWidget(e.X, e.Y);
				var index = textBlock.PointToIndex(p.X - TextPadding, p.Y - TextPadding);
				if (index != -1)
					CaretPosition = index;
			}
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
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
	}
}
