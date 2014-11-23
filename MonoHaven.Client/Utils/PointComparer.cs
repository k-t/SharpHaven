using System.Collections.Generic;
using System.Drawing;

namespace MonoHaven.Utils
{
	public class PointComparer : IComparer<Point>
	{
		public int Compare(Point first, Point second)
		{
			return (first.X == second.X)
				? first.Y - second.Y
				: first.X - second.X;
		}
	}
}
