namespace MonoHaven.UI.Remote
{
	public class ServerItemWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "ITEM";
			return new ServerItemWidget(id, parent, widget);
		}

		public ServerItemWidget(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
