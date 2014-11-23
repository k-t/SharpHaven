namespace MonoHaven.UI.Remote
{
	public class ServerLogWindow : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "LOG";
			return new ServerLogWindow(id, parent, widget);
		}

		public ServerLogWindow(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
