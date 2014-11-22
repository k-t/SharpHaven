#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Security.Cryptography;

namespace MonoHaven.Utils
{
	public static class RandomUtils
	{
		public static int GetSeed()
		{
			var cryptoResult = new byte[4];
			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(cryptoResult);
				return BitConverter.ToInt32(cryptoResult, 0);
			}
		}
	}
}
