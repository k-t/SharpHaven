using System;
using SharpHaven.Graphics;

namespace SharpHaven.Utils
{
	public static class Coord2dExtensions
	{
		public static Coord2d Add(this Coord2d a, int x, int y)
		{
			return new Coord2d(a.X + x, a.Y + y);
		}

		public static Coord2d Add(this Coord2d a, Coord2d b)
		{
			return Add(a, b.X, b.Y);
		}

		public static Coord2d Add(this Coord2d a, int value)
		{
			return new Coord2d(a.X + value, a.Y + value);
		}

		public static Coord2d Sub(this Coord2d a, Coord2d b)
		{
			return new Coord2d(a.X - b.X, a.Y - b.Y);
		}

		public static Coord2d Sub(this Coord2d a, int x, int y)
		{
			return new Coord2d(a.X - x, a.Y - y);
		}

		public static Coord2d Mul(this Coord2d a, Coord2d b)
		{
			return Mul(a, b.X, b.Y);
		}

		public static Coord2d Mul(this Coord2d a, int x, int y)
		{
			return new Coord2d(a.X * x, a.Y * y);
		}

		public static double DistanceTo(this Coord2d a, Coord2d b)
		{
			return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
		}
	}
}
