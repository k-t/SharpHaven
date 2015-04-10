using System.Collections.Generic;
using System.Drawing;

namespace SharpHaven.Utils
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
