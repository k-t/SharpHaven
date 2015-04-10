using System;

namespace SharpHaven.UI.Widgets
{
	public class ClaimRightsChangeEvent : EventArgs
	{
		private readonly int group;
		private readonly ClaimRight rights;

		public ClaimRightsChangeEvent(int group, ClaimRight rights)
		{
			this.group = group;
			this.rights = rights;
		}

		public int Group
		{
			get { return group; }
		}

		public ClaimRight Rights
		{
			get { return rights; }
		}
	}
}
