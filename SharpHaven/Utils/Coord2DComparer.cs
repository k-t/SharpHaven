using System.Collections.Generic;
using SharpHaven.Graphics;

namespace SharpHaven.Utils
{
	public class Coord2DComparer : IComparer<Coord2D>
	{
		public int Compare(Coord2D first, Coord2D second)
		{
			return (first.X == second.X)
				? first.Y - second.Y
				: first.X - second.X;
		}
	}
}
