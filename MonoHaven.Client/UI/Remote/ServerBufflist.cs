namespace MonoHaven.UI.Remote
{
	public class ServerBufflist : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "BUFFS";
			return new ServerBufflist(id, parent, widget);
		}

		public ServerBufflist(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
