using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class TextBox : Widget
	{
		private const int TextPadding = 2;

		private readonly TextBlock text;
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		public TextBox(Widget parent)
			: base(parent)
		{
			text = new TextBlock(Fonts.Default);
			text.TextColor = Color.Gray;
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
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}

		protected override void OnFocusChanged()
		{
			text.TextColor = IsFocused ? Color.White : Color.Gray;
			text.BackgroundColor = IsFocused ? Color.SteelBlue : Color.Transparent;
		}
	}
}
