using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;

namespace MonoHaven.UI.Widgets
{
	public class Label : Widget
	{
		private readonly SpriteFont font;
		private string text;
		private readonly TextLine textLine;

		public Label(Widget parent, SpriteFont font) : base(parent)
		{
			this.font = font;
			this.textLine = new TextLine(font);
			this.textLine.TextColor = Color.White;
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
				text = value ?? "";
				textLine.Clear();
				textLine.Append(text);
				if (AutoSize)
					Resize(textLine.TextWidth, font.Height);
			}
		}

		public TextAlign TextAlign
		{
			get { return textLine.TextAlign; }
			set { textLine.TextAlign = value; }
		}

		public Color BackColor
		{
			get { return textLine.BackgroundColor; }
			set { textLine.BackgroundColor = value; }
		}

		public Color TextColor
		{
			get { return textLine.TextColor; }
			set { textLine.TextColor = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(textLine, 0, 0, Width, Height);
		}
	}
}
