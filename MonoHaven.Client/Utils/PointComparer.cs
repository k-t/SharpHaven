#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
