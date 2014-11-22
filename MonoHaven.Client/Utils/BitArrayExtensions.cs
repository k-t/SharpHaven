#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Collections;

namespace MonoHaven.Utils
{
	public static class BitArrayExtensions
	{
		public static bool IsSet(this BitArray flags, int index)
		{
			return (index >= 0 && index < flags.Length) && flags[index];
		}
	}
}
