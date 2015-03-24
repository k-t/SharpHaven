using System.Drawing;
using MonoHaven.UI.Widgets;

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
			widget.Drop += (p) => SendMessage("drop", p);
			widget.Transfer += OnTransfer;
		}

		private void OnTransfer(TransferEventArgs e)
		{
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("xfer", e.Delta, mods);
		}
	}
}
