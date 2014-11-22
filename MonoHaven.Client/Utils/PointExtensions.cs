#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.Utils
{
	public static class PointExtensions
	{
		public static Point Add(this Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
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
