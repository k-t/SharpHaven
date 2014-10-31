using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;
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
			set { text.Text = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(border, 0, 0, Width, Height);
			dc.Draw(text, TextPadding, TextPadding, Width, Height);

			// draw cursor
			if (IsFocused && DateTime.Now.Millisecond > CursorBlinkRate)
			{
				dc.SetColor(Color.Black);
				dc.DrawRectangle(TextPadding + text.TextWidth, TextPadding, 1, text.Font.Height);
				dc.ResetColor();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.Handled = true;
			switch (e.Key)
			{
				case Key.BackSpace:
					if (text.TextLength != 0)
						text.Remove(text.TextLength - 1, 1);
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

			text.Append(e.KeyChar);
			e.Handled = true;
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}
	}
}
