#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Drawing;

namespace MonoHaven.Utils
{
	public static class MathUtils
	{
		public static int Div(this int n, int d)
		{
			var v = ((n < 0) ? (n + 1) : n) / d;
			if (n < 0)
				v--;
			return v;
		}

		public static int Mod(this int n, int d)
		{
			var v = n % d;
			if (v < 0)
				v += d;
			return v;
		}

		public static Point PolarToCartesian(double angle, double magnitude)
		{
			return new Point(
				(int)(Math.Cos(angle) * magnitude),
				(int)-(Math.Sin(angle) * magnitude));
		}
	}
}
