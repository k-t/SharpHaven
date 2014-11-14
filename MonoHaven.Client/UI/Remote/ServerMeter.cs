namespace MonoHaven.UI.Remote
{
	public class ServerMeter : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "METER";
			return new ServerMeter(id, parent, widget);
		}

		public ServerMeter(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
