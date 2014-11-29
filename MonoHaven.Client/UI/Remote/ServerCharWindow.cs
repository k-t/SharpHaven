namespace MonoHaven.UI.Remote
{
	public class ServerCharWindow : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Window(parent.Widget, "Character Sheet");
			return new ServerCharWindow(id, parent, widget);
		}

		public ServerCharWindow(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
