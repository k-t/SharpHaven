﻿using System.Drawing;
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
			get { return text.Value; }
			set { text.Value = value; }
		}

		public Color TextColor
		{
			get { return text.Color; }
			set { text.Color = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(text, 0, 0, Width, Height);
		}
	}
}
