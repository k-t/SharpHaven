using System;
using C5;

namespace MonoHaven
{
	public static class Random
	{
		private static readonly C5Random random = new C5Random(DateTime.Now.Ticks);

		public static int Next()
		{
			return random.Next();
		}

		public static int Next(int maxValue)
		{
			return random.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return random.Next(minValue, maxValue);
		}
	}
}
