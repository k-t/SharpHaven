using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics.Text;

namespace MonoHaven.Graphics.Sprites
{
	public class FloatText : ISprite
	{
		private readonly TextLine text;
		private readonly Picture part;
		private readonly int startY;
		private double a;

		public FloatText(string text, Color color)
		{
			this.text = new TextLine(Fonts.LabelText);
			this.text.TextColor = color;
			this.text.Append(text);
			this.startY = -Fonts.LabelText.Height;
			this.part = new Picture(this.text, new Point(this.text.TextWidth / 2, startY), 5, 0);
		}

		public IEnumerable<Picture> Parts
		{
			get { yield return part; }
		}

		public bool Tick(int dt)
		{
			a += dt / 2000.0D;
			UpdateOffset();

			// TODO: make sprites disposable
			if (a >= 1.0D)
			{
				text.Dispose();
				return true;
			}

			return false;
		}

		private void UpdateOffset()
		{
			int y = startY - (int)(10.0D * a);
			part.Offset = new Point(part.Offset.X, y);
		}
	}
}
