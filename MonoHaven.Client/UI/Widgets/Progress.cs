using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;

namespace MonoHaven.UI.Widgets
{
	public class Progress : Widget
	{
		private readonly TextLine textLine;
		private int value;

		public Progress(Widget parent) : base(parent)
		{
			textLine = new TextLine(Fonts.LabelText);
			textLine.TextColor = Color.Yellow;
			textLine.TextAlign = TextAlign.Center;
			textLine.SetWidth(75);
			Resize(textLine.Width, 20);
		}

		public int Value
		{
			get { return value; }
			set
			{
				this.value = value;
				textLine.Clear();
				textLine.Append(string.Format("{0}%", value));
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(textLine, 0, 0);
		}

		protected override void OnDispose()
		{
			if (textLine != null)
				textLine.Dispose();
		}
	}
}
