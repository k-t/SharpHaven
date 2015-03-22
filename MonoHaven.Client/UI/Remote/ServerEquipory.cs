using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerEquipory : ServerWindow
	{
		public static new ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Equipory(parent.Widget, parent.Session.State.Objects);
			return new ServerEquipory(id, parent, widget);
		}

		private readonly Equipory widget;

		public ServerEquipory(ushort id, ServerWidget parent, Equipory widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Drop += (slot) => SendMessage("drop", slot);
			this.widget.ItemTake += (slot, p) => SendMessage("take", slot, p);
			this.widget.ItemTransfer += (slot, p) => SendMessage("transfer", slot, p);
			this.widget.ItemInteract += (slot, p) => SendMessage("iact", slot, p);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "ava")
				widget.SetGob((int)args[0]);
			else if (message == "set")
			{
				int i = 0;
				int j = 0;
				while (j < args.Length)
				{
					var resId = (int)args[j++];
					if (resId >= 0)
					{
						var q = (int)args[j++];
						var tooltip = (j < args.Length && args[j] is string)
							? (string)args[j++]
							: null;

						var itemMold = Session.Get<ItemMold>(resId);
						var item = new Item(itemMold);
						item.Quality = q;
						if (!string.IsNullOrEmpty(tooltip))
							item.Tooltip = tooltip;

						widget.SetItem(i, item);
					}
					else
						widget.SetItem(i, null);
					i++;
				}
			}
			else if (message == "setres")
			{
				var i = (int)args[0];
				var resId = (int)args[1];
				var q = (int)args[2];

				var item = new Item(Session.Get<ItemMold>(resId));
				item.Quality = q;

				// TODO: tooltip must be preserved
				widget.SetItem(i, item);
			}
			else
				base.ReceiveMessage(message, args);
		}
	}
}
