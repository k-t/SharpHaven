using System;
using System.Drawing;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.Utils;

namespace SharpHaven.Game
{
	public class GobSpeech : IDisposable
	{
		private const int Padding = 4;

		private static readonly Drawable bubble;

		static GobSpeech()
		{
			bubble = App.Resources.Get<Drawable>("custom/ui/speech-bubble");
		}

		private TextLine text;
		private Point offset;

		public GobSpeech(string text, Point offset)
		{
			this.text = new TextLine(Fonts.Text);
			this.text.TextColor = Color.Black;
			this.text.Append(text);
			this.offset = offset;
		}

		public void Draw(DrawingContext dc, int x, int y)
		{
			var p = offset.Add(x - 3, y - 8 - Fonts.Text.Height);
			var w = Math.Max(text.TextWidth + Padding * 2, 10);
			var h = Fonts.Text.Height + Padding * 2;

			dc.Draw(bubble, p.X, p.Y, w, h + 2);
			dc.Draw(text, p.X + Padding, p.Y + Padding);
		}

		public void Dispose()
		{
			text.Dispose();
		}
	}
}
