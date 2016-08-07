using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerEquipory : ServerWindow
	{
		private Equipory widget;

		public ServerEquipory(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("ava", args => widget.SetGob((int)args[0]));
			SetHandler("set", Set);
			SetHandler("setres", SetResource);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static new ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerEquipory(id, parent);
		}

		protected override void OnInit(Coord2D position, object[] args)
		{
			widget = new Equipory(Parent.Widget, Parent.Session.Objects);
			widget.Move(position);
			widget.Drop += (slot) => SendMessage("drop", slot);
			widget.ItemTake += (slot, p) => SendMessage("take", slot, p);
			widget.ItemTransfer += (slot, p) => SendMessage("transfer", slot, p);
			widget.ItemAct += (slot, p) => SendMessage("iact", slot, p);
			widget.ItemInteract += (slot) => SendMessage("itemact", slot);
			widget.Closed += () => SendMessage("close");
		}

		private void Set(object[] args)
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

					var itemMold = Session.Resources.Get<ItemProto>(resId);
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

		private void SetResource(object[] args)
		{
			var i = (int)args[0];
			var resId = (int)args[1];
			var q = (int)args[2];

			var item = new Item(Session.Resources.Get<ItemProto>(resId));
			item.Quality = q;

			// TODO: tooltip must be preserved
			widget.SetItem(i, item);
		}
	}
}
