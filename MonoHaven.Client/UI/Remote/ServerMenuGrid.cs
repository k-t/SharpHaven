using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new MenuGrid(parent.Widget, parent.Session.State.Actions);
			return new ServerMenuGrid(id, parent, widget);
		}

		public ServerMenuGrid(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
