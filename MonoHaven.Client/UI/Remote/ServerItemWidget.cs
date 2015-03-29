using System.Drawing;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerItemWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			int i = 3;

			var resId = (int)args[0];
			var q = (int)args[1];
			var dragOffset = (int)args[2] != 0 ? (Point?)args[i++] : null;
			var tooltip = args.Length > i ? (string)args[i++] : null;
			var num = args.Length > i ? (int)args[i] : -1;

			var itemMold = parent.Session.Get<ItemMold>(resId);
			var item = new Item(itemMold);
			item.Quality = q;
			item.Amount = num;
			if (!string.IsNullOrEmpty(tooltip))
				item.Tooltip = tooltip;

			var widget = new ItemWidget(parent.Widget, dragOffset);
			widget.Item = item;
			return new ServerItemWidget(id, parent, widget);
		}

		private readonly ItemWidget widget;

		public ServerItemWidget(ushort id, ServerWidget parent, ItemWidget widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Take += (p) => SendMessage("take", p);
			this.widget.Transfer += (p) => SendMessage("transfer", p);
			this.widget.Drop += (p) => SendMessage("drop", p);
			this.widget.Act += (p) => SendMessage("iact", p);
			this.widget.Interact += (mods) => SendMessage("itemact", ServerInput.ToServerModifiers(mods));
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "tt")
			{
				var text = args.Length > 0 ? (string)args[0] : null;
				widget.Tooltip = !string.IsNullOrEmpty(text)
					? new Tooltip(text)
					: null;
			}
			else if (message == "chres")
			{
				int resId = (int)args[0];
				int q = (int)args[1];
				var item = new Item(Session.Get<ItemMold>(resId));
				item.Quality = q;
				widget.Item = item;
			}
			else if (message == "meter")
				widget.Item.Meter = (int)args[0];
			else if (message == "num")
				widget.Item.Amount = (int)args[0];
			// TODO:
			//else if (message == "color")
			//	widget.OverlayColor = (Color)args[0];
			else
				base.ReceiveMessage(message, args);
		}
	}
}
