using System.Drawing;

namespace MonoHaven.Utils
{
	public static class PointUtils
	{
		public static Point Add(this Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point Sub(this Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}
	}
}
