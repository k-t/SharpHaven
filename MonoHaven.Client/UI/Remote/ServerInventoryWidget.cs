namespace MonoHaven.UI.Remote
{
	public class ServerInventoryWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "INV";
			return new ServerInventoryWidget(id, parent, widget);
		}

		public ServerInventoryWidget(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
