using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class TabWidget : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable tabImage;
		
		static TabWidget()
		{
			box = App.Resources.GetImage("custom/ui/wbox2");
			tabImage = App.Resources.GetImage("custom/ui/tab");
		}

		private readonly List<Tab> tabs;
		private int tabPosition = 15;

		public TabWidget(Widget parent) : base(parent)
		{
			tabs = new List<Tab>();
		}

		public void AddTab(string text, Widget widget)
		{
			AddChild(widget);
			tabs.Add(new Tab(text, widget));
			widget.Move(15, tabPosition);
			tabPosition += 15;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int x = 0;
			int tabHeight = 0;
			foreach (var tab in tabs)
			{
				tab.Draw(dc, x, 0);
				x += tab.Width - 1;
				tabHeight = Math.Max(tabHeight, tab.Height);
			}
			dc.Draw(box, 0, tabHeight, Width, Height - tabHeight);
		}

		protected override void OnDispose()
		{
			foreach (var tab in tabs)
				tab.Text.Dispose();
		}

		private class Tab
		{
			private const int TextPadding = 2;

			private readonly TextBlock text;

			public Tab(string text, Widget widget)
			{
				this.text = new TextBlock(Fonts.Default);
				this.text.TextColor = Color.White;
				this.text.Append(text);
				Widget = widget;
			}

			public int Width
			{
				get { return text.TextWidth + TextPadding * 2; }
			}

			public int Height
			{
				get { return text.Font.Height + TextPadding * 2; }
			}

			public TextBlock Text
			{
				get { return text; }
			}

			public Widget Widget
			{
				get;
				set;
			}

			public void Draw(DrawingContext dc, int x, int y)
			{
				dc.Draw(tabImage, x, y, Width, Height);
				dc.Draw(text, x + TextPadding, y + TextPadding, Text.TextWidth, text.Font.Height);
			}
		}
	}
}
