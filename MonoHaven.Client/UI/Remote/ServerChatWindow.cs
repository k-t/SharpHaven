namespace MonoHaven.UI.Remote
{
	public class ServerChatWindow : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "CHAT";
			return new ServerChatWindow(id, parent, widget);
		}

		public ServerChatWindow(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
