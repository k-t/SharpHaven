namespace MonoHaven.UI.Remote
{
	public class ServerBuddyList : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Window(parent.Widget, "Kin");
			return new ServerBuddyList(id, parent, widget);
		}

		public ServerBuddyList(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
