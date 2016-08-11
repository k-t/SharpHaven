using System.Collections.Generic;
using Haven;

namespace SharpHaven.Utils
{
	public class Coord2DComparer : IComparer<Point2D>
	{
		public int Compare(Point2D first, Point2D second)
		{
			return (first.X == second.X)
				? first.Y - second.Y
				: first.X - second.X;
		}
	}
}
