using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using OpenTK;
using QuickFont;

namespace MonoHaven.UI.Widgets
{
	public class TextBox : Widget
	{
		private readonly QFont font;
		private string text;
		private ProcessedText processedText;
		private readonly Texture borderTexture;
		private readonly NinePatch border;

		public TextBox()
		{
			font = Fonts.Text;
			Text = string.Empty;
			borderTexture = new Texture(EmbeddedResource.GetImage("textbox.png"));
			border = new NinePatch(borderTexture, 2, 2, 2, 2);
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				processedText = font.ProcessText(text, float.MaxValue, QFontAlignment.Left);
			}
		}

		protected override void OnDraw(DrawingContext g)
		{
			g.Draw(border, 0, 0, Width, Height);
			g.Draw(font, Color.Black, processedText, 3, 3);
		}

		protected override void OnDispose()
		{
			borderTexture.Dispose();
		}
	}
}
