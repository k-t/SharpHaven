using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class Progress : Widget
	{
		private readonly TextBlock textBlock;
		private int value;

		public Progress(Widget parent) : base(parent)
		{
			textBlock = new TextBlock(Fonts.LabelText);
			textBlock.TextColor = Color.DeepPink;
			textBlock.TextAlign = TextAlign.Center;
			textBlock.SetWidth(75);
			Resize(textBlock.Width, 20);
		}

		public int Value
		{
			get { return value; }
			set
			{
				this.value = value;
				textBlock.Clear();
				textBlock.Append(string.Format("{0}%", value));
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(textBlock, 0, 0);
		}

		protected override void OnDispose()
		{
			if (textBlock != null)
				textBlock.Dispose();
		}
	}
}
