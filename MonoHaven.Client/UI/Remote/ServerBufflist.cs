namespace MonoHaven.UI.Remote
{
	public class ServerBufflist : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Bufflist(parent.Widget, parent.Session.State);
			return new ServerBufflist(id, parent, widget);
		}

		public ServerBufflist(ushort id, ServerWidget parent, Bufflist widget)
			: base(id, parent, widget)
		{
		}
	}
}
