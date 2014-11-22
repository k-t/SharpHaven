﻿#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Utils
{
	public static class MathExtensions
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
	}
}
