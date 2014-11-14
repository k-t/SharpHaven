namespace MonoHaven.UI.Remote
{
	public class ServerSpeedget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "SPEED";
			return new ServerSpeedget(id, parent, widget);
		}

		public ServerSpeedget(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
