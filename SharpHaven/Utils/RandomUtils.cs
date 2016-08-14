using System;
using System.Security.Cryptography;
using Haven;

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

		public static int GetSeed(Point2D p)
		{
			return p.X ^ p.Y;
		}

		public static double NextGaussian(this Random rnd)
		{
			// uniform(0,1) random doubles
			var u1 = rnd.NextDouble();
			var u2 = rnd.NextDouble();
			// random normal(0,1)
			return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		}
	}
}
