using System.Drawing;
using MonoHaven.Graphics;
using OpenTK.Graphics;
using QuickFont;

namespace MonoHaven.UI.Widgets
{
	public class Label : Widget
	{
		private readonly QFont font;
		private string text;
		private Color textColor;
		private ProcessedText processedText;

		public Label()
		{
			font = Fonts.Text;
			Text = string.Empty;
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

		public Color TextColor
		{
			get { return textColor; }
			set { textColor = value; }
		}

		protected override void OnDraw(DrawingContext g)
		{
			g.Draw(font, textColor, processedText, 0, 0);
		}
	}
}
