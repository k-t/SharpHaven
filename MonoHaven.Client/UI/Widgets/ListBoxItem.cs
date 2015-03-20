using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class ListBoxItem
	{
		private readonly Drawable image;
		private readonly string text;
		private readonly Color textColor;

		public ListBoxItem(Drawable image, string text)
			: this(image, text, Color.White)
		{
		}

		public ListBoxItem(Drawable image, string text, Color textColor)
		{
			this.image = image;
			this.text = text;
			this.textColor = textColor;
		}

		public Drawable Image
		{
			get { return image; }
		}

		public string Text
		{
			get { return text; }
		}

		public Color TextColor
		{
			get { return textColor; }
		}
	}
}
