namespace MonoHaven.UI.Remote
{
	public class ServerCalendar : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "CAL";
			return new ServerCalendar(id, parent, widget);
		}

		public ServerCalendar(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
