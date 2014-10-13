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
