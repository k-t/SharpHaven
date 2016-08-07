using System.Collections.Generic;
using SharpHaven.Graphics;

namespace SharpHaven.Utils
{
	public class Coord2dComparer : IComparer<Coord2d>
	{
		public int Compare(Coord2d first, Coord2d second)
		{
			return (first.X == second.X)
				? first.Y - second.Y
				: first.X - second.X;
		}
	}
}
