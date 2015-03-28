using System;
using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class ListBoxItem : IDisposable
	{
		private readonly TextLine label;
		private string labelText;

		public ListBoxItem(Drawable image, string labelText)
			: this(image, labelText, Color.White)
		{
		}

		public ListBoxItem(Drawable image, string labelText, Color textColor)
		{
			this.Image = image;
			this.labelText = labelText;

			label = new TextLine(Fonts.LabelText);
			label.TextColor = textColor;
			label.Append(labelText);
		}

		public Drawable Image
		{
			get;
			set;
		}

		public string Text
		{
			get { return labelText; }
			set
			{
				labelText = value;
				label.Clear();
				label.Append(value);
			}
		}

		public Color TextColor
		{
			get { return label.TextColor; }
			set { label.TextColor = value; }
		}

		public object Tag
		{
			get;
			set;
		}

		public void Draw(DrawingContext dc, int x, int y, int itemHeight)
		{
			if (Image != null)
				dc.Draw(Image, x, y, itemHeight, itemHeight);
			dc.Draw(label, x + itemHeight + 2, y + (itemHeight - label.Font.Height) / 2);
		}

		public void Dispose()
		{
			if (label != null)
				label.Dispose();
		}
	}
}
