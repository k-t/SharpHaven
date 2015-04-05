using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;

namespace MonoHaven.UI
{
	public class Tooltip : IDisposable
	{
		private static readonly Drawable background;

		static Tooltip()
		{
			background = App.Resources.Get<Drawable>("custom/ui/tooltip-bg");
		}

		private readonly TextLine textLine;

		public Tooltip(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				textLine = new TextLine(Fonts.Tooltip);
				textLine.TextColor = Color.White;
				textLine.Append(text);
			}
		}

		public void Draw(DrawingContext dc, int x, int y)
		{
			if (textLine == null)
				return;

			int w = textLine.TextWidth;
			int h = textLine.Font.Height;
			
			x = Math.Max(x - w, 0);
			y = Math.Max(y - h, 0);

			dc.Draw(background, x - 3, y - 3, w + 6, h + 6);
			dc.Draw(textLine, x, y);
		}

		public void Dispose()
		{
			if (textLine != null)
				textLine.Dispose();
		}
	}
}
