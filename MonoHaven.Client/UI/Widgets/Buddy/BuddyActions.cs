using System;

namespace SharpHaven.UI.Widgets
{
	[Flags]
	public enum BuddyActions
	{
		Forget = 1,
		PrivateChat = 2,
		EndKinship = 4,
		Invite = 8,
		Describe = 16,
		Exile = 32
	}
}
