using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class Label : Widget
	{
		private readonly SpriteFont font;
		private string text;
		private readonly TextBlock textBlock;

		public Label(Widget parent, SpriteFont font) : base(parent)
		{
			this.font = font;
			this.textBlock = new TextBlock(font);
			this.textBlock.TextColor = Color.White;
			Resize(0, font.Height);
		}

		public Label(Widget parent) : this(parent, Fonts.Default)
		{
		}

		public bool AutoSize
		{
			get;
			set;
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				textBlock.Clear();
				textBlock.Append(text);
				if (AutoSize)
					Resize(textBlock.TextWidth, font.Height);
			}
		}

		public TextAlign TextAlign
		{
			get { return textBlock.TextAlign; }
			set { textBlock.TextAlign = value; }
		}

		public Color BackColor
		{
			get { return textBlock.BackgroundColor; }
			set { textBlock.BackgroundColor = value; }
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
