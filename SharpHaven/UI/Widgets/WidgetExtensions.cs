using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public static class WidgetExtensions
	{
		public static Widget Move(this Widget widget, int x, int y)
		{
			return widget.Move(new Coord2d(x, y));
		}

		public static Widget Move(this Widget widget, Coord2d p)
		{
			widget.Position = p;
			return widget;
		}

		public static Widget Resize(this Widget widget, int width, int height)
		{
			return widget.Resize(new Coord2d(width, height));
		}

		public static Widget Resize(this Widget widget, Coord2d size)
		{
			widget.Size = size;
			return widget;
		}
	}
}
