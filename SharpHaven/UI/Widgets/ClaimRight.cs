using System;

namespace SharpHaven.UI.Widgets
{
	[Flags]
	public enum ClaimRight
	{
		Trespassing = 1,
		Theft = 2,
		Vandalism = 4
	}
}