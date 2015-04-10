using System.Collections;

namespace SharpHaven.Utils
{
	public static class BitArrayExtensions
	{
		public static bool IsSet(this BitArray flags, int index)
		{
			return (index >= 0 && index < flags.Length) && flags[index];
		}
	}
}
