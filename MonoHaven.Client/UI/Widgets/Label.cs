using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class Label : Widget
	{
		private readonly Text text;

		public Label()
		{
			text = new Text(Fonts.Text);
		}

		public string Text
		{
			get { return text.Value; }
			set { text.Value = value; }
		}

		public Color TextColor
		{
			get { return text.Color; }
			set { text.Color = value; }
		}

		public override void Draw(DrawingContext g)
		{
			g.Draw(text, 0, 0);
		}

		protected override void OnDispose()
		{
			text.Dispose();
		}
	}
}
