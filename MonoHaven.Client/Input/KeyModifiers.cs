using System;

namespace MonoHaven.Input
{
	[Flags]
	public enum KeyModifiers
	{
		None = 0,
		Shift = 1,
		Control = 2,
		Alt = 4,
		Super = 8
	}
}
