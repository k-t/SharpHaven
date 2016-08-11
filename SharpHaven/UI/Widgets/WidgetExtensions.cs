using Haven;

namespace SharpHaven.UI.Widgets
{
	public static class WidgetExtensions
	{
		public static Widget Move(this Widget widget, int x, int y)
		{
			return widget.Move(new Point2D(x, y));
		}

		public static Widget Move(this Widget widget, Point2D p)
		{
			widget.Position = p;
			return widget;
		}

		public static Widget Resize(this Widget widget, int width, int height)
		{
			return widget.Resize(new Point2D(width, height));
		}

		public static Widget Resize(this Widget widget, Point2D size)
		{
			widget.Size = size;
			return widget;
		}
	}
}
