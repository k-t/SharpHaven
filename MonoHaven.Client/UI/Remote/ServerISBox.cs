using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerISBox : ServerWidget
	{
		private ISBox widget;

		public ServerISBox(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("chnum", SetAmounts);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerISBox(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			// TODO: get tooltip
			var image = App.Resources.Get<Drawable>((string)args[0]);
			int remaining = (int)args[1];
			int available = (int)args[2];
			int built = (int)args[3];

			widget = new ISBox(Parent.Widget);
			widget.Move(position);
			widget.Image = image;
			widget.Remaining = remaining;
			widget.Available = available;
			widget.Built = built;

			widget.ItemDrop += () => SendMessage("drop");
			widget.ItemInteract += () => SendMessage("iact");
			widget.Click += () => SendMessage("click");
			widget.Transfer += () => SendMessage("xfer");
			widget.Transfer2 += OnTransfer2;
		}

		private void SetAmounts(object[] args)
		{
			widget.Remaining = (int)args[0];
			widget.Available = (int)args[1];
			widget.Built = (int)args[2];
		}

		private void OnTransfer2(TransferEventArgs e)
		{
			int mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("xfer2", e.Delta, mods);
		}
	}
}
