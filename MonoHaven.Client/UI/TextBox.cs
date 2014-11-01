using System;
using System.Drawing;
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

		private readonly TextBlock text;
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		private int caretPosition;

		public TextBox(Widget parent)
			: base(parent)
		{
			text = new TextBlock(Fonts.Default);
			text.TextColor = Color.Black;
			borderTexture = new Texture(EmbeddedResource.GetImage("textbox.png"));
			border = new NinePatch(borderTexture, 2, 2, 2, 2);

			IsFocusable = true;
		}

		public string Text
		{
			get { return text.Text; }
			set
			{
				text.Text = value;
				caretPosition = text.TextLength;
			}
		}

		private int CaretPosition
		{
			get { return caretPosition; }
			set { caretPosition = MathHelper.Clamp(value, 0, text.TextLength); }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(border, 0, 0, Width, Height);
			dc.Draw(text, TextPadding, TextPadding, Width, Height);

			// draw cursor
			if (IsFocused && DateTime.Now.Millisecond > CursorBlinkRate)
			{
				int cx = caretPosition < text.TextLength
					? text.Glyphs[caretPosition].Position.X
					: text.TextWidth;

				dc.SetColor(Color.Black);
				dc.DrawRectangle(TextPadding + cx, TextPadding, 1, text.Font.Height);
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
						text.Remove(caretPosition - 1, 1);
						CaretPosition--;
					}
					break;
				case Key.Delete:
					if (caretPosition < text.TextLength)
						text.Remove(caretPosition, 1);
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

			text.Insert(caretPosition, e.KeyChar);
			CaretPosition++;
			e.Handled = true;
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}

		private void MoveCaret(int offset)
		{
			caretPosition += offset;
			caretPosition = MathHelper.Clamp(caretPosition, 0, text.TextLength);
		}
	}
}
