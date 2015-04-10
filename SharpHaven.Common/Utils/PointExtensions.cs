using System;
using System.Drawing;

namespace SharpHaven.Utils
{
	public static class PointExtensions
	{
		public static Point Add(this Point a, int x, int y)
		{
			return new Point(a.X + x, a.Y + y);
		}

		public static Point Add(this Point a, Point b)
		{
			return Add(a, b.X, b.Y);
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

		public static Point Mul(this Point a, Point b)
		{
			return Mul(a, b.X, b.Y);
		}

		public static Point Mul(this Point a, int x, int y)
		{
			return new Point(a.X * x, a.Y * y);
		}

		public static double DistanceTo(this Point a, Point b)
		{
			return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
		}
	}
}
