namespace MonoHaven.UI.Remote
{
	public class ServerHud : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "HUD";
			return new ServerHud(id, parent, widget);
		}

		public ServerHud(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
