using System;
using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Tooltip : IDisposable
	{
		private static readonly Drawable background;

		static Tooltip()
		{
			background = App.Resources.GetImage("custom/ui/tooltip-bg");
		}

		private readonly TextBlock textBlock;

		public Tooltip(string text)
		{
			textBlock = new TextBlock(Fonts.Tooltip);
			textBlock.TextColor = Color.White;
			textBlock.Append(text);
		}

		public void Draw(DrawingContext dc, int x, int y)
		{
			int w = textBlock.TextWidth;
			int h = textBlock.Font.Height;
			
			x = Math.Max(x - w, 0);
			y = Math.Max(y - h, 0);

			dc.Draw(background, x - 3, y - 3, w + 6, h + 6);
			dc.Draw(textBlock, x, y);
		}

		public void Dispose()
		{
			if (textBlock != null)
				textBlock.Dispose();
		}
	}
}
