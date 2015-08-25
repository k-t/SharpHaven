using System.Drawing;

namespace SharpHaven.UI.Widgets
{
	public static class WidgetExtensions
	{
		public static Widget Move(this Widget widget, int x, int y)
		{
			return widget.Move(new Point(x, y));
		}

		public static Widget Move(this Widget widget, Point p)
		{
			widget.Position = p;
			return widget;
		}

		public static Widget Resize(this Widget widget, int width, int height)
		{
			return widget.Resize(new Size(width, height));
		}

		public static Widget Resize(this Widget widget, Size size)
		{
			widget.Size = size;
			return widget;
		}
	}
}
