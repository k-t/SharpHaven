using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Label : Widget
	{
		private readonly TextBlock text;

		public Label(Widget parent)
			: base(parent)
		{
			text = new TextBlock(Fonts.Default);
		}

		public string Text
		{
			get { return text.Text.ToString(); }
			set
			{
				text.Text.Clear();
				text.Text.Append(value);
			}
		}

		public TextAlign TextAlign
		{
			get { return text.TextAlign; }
			set { text.TextAlign = value; }
		}

		public Color TextColor
		{
			get { return text.TextColor; }
			set { text.TextColor = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(text, 0, 0, Width, Height);
		}
	}
}
