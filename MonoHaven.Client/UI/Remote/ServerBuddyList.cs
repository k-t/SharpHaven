using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerBuddyList : ServerWindow
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new BuddyList(parent.Widget);
			return new ServerBuddyList(id, parent, widget);
		}

		public ServerBuddyList(ushort id, ServerWidget parent, BuddyList widget)
			: base(id, parent, widget)
		{
		}
	}
}
