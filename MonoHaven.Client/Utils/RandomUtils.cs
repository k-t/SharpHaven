using System;
using System.Drawing;
using System.Security.Cryptography;

namespace SharpHaven.Utils
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

		public static long GetSeed(Point p)
		{
			return p.X ^ p.Y;
		}
	}
}
