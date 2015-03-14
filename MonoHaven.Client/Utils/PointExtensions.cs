using System.Drawing;

namespace MonoHaven.Utils
{
	public static class PointExtensions
	{
		public static Point Add(this Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point Add(this Point a, int value)
		{
			return new Point(a.X + value, a.Y + value);
		}

		public static Point Sub(this Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}

		public static Point Sub(this Point a, int x, int y)
		{
			return new Point(a.X - x, a.Y - y);
		}
	}
}
