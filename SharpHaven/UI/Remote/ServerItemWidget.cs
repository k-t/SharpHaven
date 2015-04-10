using System.Drawing;
using SharpHaven.Game;
using SharpHaven.UI.Widgets;
using SharpHaven.Utils;

namespace SharpHaven.UI.Remote
{
	public class ServerItemWidget : ServerWidget
	{
		private ItemWidget widget;

		public ServerItemWidget(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("tt", SetTooltip);
			SetHandler("chres", SetResource);
			SetHandler("meter", SetMeter);
			SetHandler("num", SetAmount);
			// TODO: SetHandler("color")
			// widget.OverlayColor = (Color)args[0];
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerItemWidget(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			int i = 3;

			var resId = (int)args[0];
			var q = (int)args[1];
			var dragOffset = (int)args[2] != 0 ? (Point?)args[i++] : null;
			var tooltip = args.Length > i ? (string)args[i++] : null;
			var num = args.Length > i ? (int)args[i] : -1;

			var itemMold = Parent.Session.Get<ItemMold>(resId);
			var item = new Item(itemMold);
			item.Quality = q;
			item.Amount = num;
			if (!string.IsNullOrEmpty(tooltip))
				item.Tooltip = tooltip;

			widget = new ItemWidget(Parent.Widget, dragOffset);
			widget.Take += (p) => SendMessage("take", p);
			widget.Transfer += (p) => SendMessage("transfer", p);
			widget.Drop += (p) => SendMessage("drop", p);
			widget.Act += (p) => SendMessage("iact", p);
			widget.Interact += (mods) => SendMessage("itemact", ServerInput.ToServerModifiers(mods));
			widget.Item = item;

			if (dragOffset.HasValue)
				widget.Move(Session.State.Screen.MousePosition.Sub(dragOffset.Value));
			else
				widget.Move(position);
		}

		private void SetAmount(object[] args)
		{
			widget.Item.Amount = (int)args[0];
		}

		private void SetMeter(object[] args)
		{
			widget.Item.Meter = (int)args[0];
		}

		private void SetResource(object[] args)
		{
			int resId = (int)args[0];
			int q = (int)args[1];
			var item = new Item(Session.Get<ItemMold>(resId));
			item.Quality = q;
			widget.Item = item;
		}

		private void SetTooltip(object[] args)
		{
			var text = args.Length > 0 ? (string)args[0] : null;
			widget.Tooltip = !string.IsNullOrEmpty(text)
				? new Tooltip(text)
				: null;
		}
	}
}
