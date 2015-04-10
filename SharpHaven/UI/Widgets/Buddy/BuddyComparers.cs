using System;

namespace SharpHaven.UI.Widgets
{
	public static class BuddyComparison
	{
		public static readonly Comparison<Buddy> Alphabetical =
			(a, b) => string.Compare(a.Name, b.Name);

		public static readonly Comparison<Buddy> ByGroup =
			(a, b) =>
			{
				if (a.Group == b.Group) return Alphabetical(a, b);
				return a.Group - b.Group;
			};

		public static readonly Comparison<Buddy> ByStatus =
			(a, b) =>
			{
				if (a.OnlineStatus == b.OnlineStatus) return Alphabetical(a, b);
				return b.OnlineStatus - a.OnlineStatus;
			};
	}
}
