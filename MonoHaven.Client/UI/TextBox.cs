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
			text.Color = Color.Black;
			borderTexture = new Texture(EmbeddedResource.GetImage("textbox.png"));
			border = new NinePatch(borderTexture, 2, 2, 2, 2);
		}

		public string Text
		{
			get { return text.Value; }
			set { text.Value = value; }
		}

		protected override void OnDraw(DrawingContext g)
		{
			g.Draw(border, 0, 0, Width, Height);
			g.Draw(text, 3, 4, Width, Height);
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}
	}
}
