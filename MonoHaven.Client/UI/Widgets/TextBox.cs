using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI.Widgets
{
	public class TextBox : Widget
	{
		private readonly Text text;
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		public TextBox()
		{
			text = new Text(Fonts.Text);
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
			g.Draw(text, 0, 0);
		}

		protected override void OnDispose()
		{
			text.Dispose();
			borderTexture.Dispose();
		}
	}
}
