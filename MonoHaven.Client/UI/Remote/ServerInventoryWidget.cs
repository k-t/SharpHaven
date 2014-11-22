using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerInventoryWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var widget = new InventoryWidget(parent.Widget);
			widget.SetInventorySize(size);
			return new ServerInventoryWidget(id, parent, widget);
		}

		public ServerInventoryWidget(ushort id, ServerWidget parent, InventoryWidget widget)
			: base(id, parent, widget)
		{
		}
	}
}
