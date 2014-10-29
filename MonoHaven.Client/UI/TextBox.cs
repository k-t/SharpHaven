using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class TextBox : Widget
	{
		private readonly TextBlock text;
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		public TextBox()
		{
			text = new TextBlock(Fonts.Default);
			text.Color = Color.Gray;
			borderTexture = new Texture(EmbeddedResource.GetImage("textbox.png"));
			border = new NinePatch(borderTexture, 2, 2, 2, 2);

			IsFocusable = true;
		}

		public string Text
		{
			get { return text.Value; }
			set { text.Value = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(border, 0, 0, Width, Height);
			dc.Draw(text, 3, 4, Width, Height);
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}

		protected override void OnGotFocus()
		{
			text.Color = Color.Black;
		}

		protected override void OnLostFocus()
		{
			text.Color = Color.Gray;
		}
	}
}
