using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Label : Widget
	{
		private string text;
		private readonly TextBlock textBlock;

		public Label(Widget parent, SpriteFont font) : base(parent)
		{
			textBlock = new TextBlock(font);
			textBlock.TextColor = Color.White;
			SetSize(0, font.Height);
		}

		public Label(Widget parent) : this(parent, Fonts.Default)
		{
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				textBlock.Clear();
				textBlock.Append(text);
			}
		}

		public TextAlign TextAlign
		{
			get { return textBlock.TextAlign; }
			set { textBlock.TextAlign = value; }
		}

		public Color TextColor
		{
			get { return textBlock.TextColor; }
			set { textBlock.TextColor = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(textBlock, 0, 0, Width, Height);
		}
	}
}
