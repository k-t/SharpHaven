using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerInventoryWidget : ServerWidget
	{
		private InventoryWidget widget;

		public ServerInventoryWidget(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("sz", SetSize);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerInventoryWidget(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var size = (Point)args[0];

			widget = new InventoryWidget(Parent.Widget);
			widget.Move(position);
			widget.SetInventorySize(size);
			widget.Drop += (p) => SendMessage("drop", p);
			widget.Transfer += OnTransfer;
		}

		private void SetSize(object[] args)
		{
			widget.SetInventorySize((Point)args[0]);
		}

		private void OnTransfer(TransferEvent e)
		{
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("xfer", e.Delta, mods);
		}
	}
}
