using System;

namespace SharpHaven.UI.Widgets
{
	public class ClaimRightsChangeEvent : EventArgs
	{
		public ClaimRightsChangeEvent(int group, ClaimRight rights)
		{
			Group = group;
			Rights = rights;
		}

		public int Group { get; }

		public ClaimRight Rights { get; }
	}
}
