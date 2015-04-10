using OpenTK.Input;

namespace SharpHaven.Utils
{
	public static class KeyModifiersExtensions
	{
		public static bool HasShift(this KeyModifiers mods)
		{
			return mods.HasFlag(KeyModifiers.Shift);
		}

		public static bool HasControl(this KeyModifiers mods)
		{
			return mods.HasFlag(KeyModifiers.Control);
		}

		public static bool HasAlt(this KeyModifiers mods)
		{
			return mods.HasFlag(KeyModifiers.Alt);
		}
	}
}
