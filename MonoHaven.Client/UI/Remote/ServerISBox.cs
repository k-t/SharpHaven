using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerISBox : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			// TODO: get tooltip
			var image = App.Resources.Get<Drawable>((string)args[0]);
			int remaining = (int)args[1];
			int available = (int)args[2];
			int built = (int)args[3];

			var widget = new ISBox(parent.Widget);
			widget.Image = image;
			widget.Remaining = remaining;
			widget.Available = available;
			widget.Built = built;
			return new ServerISBox(id, parent, widget);
		}

		private ISBox widget;

		public ServerISBox(ushort id, ServerWidget parent, ISBox widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.ItemDrop += () => SendMessage("drop");
			this.widget.ItemInteract += () => SendMessage("iact");
			this.widget.Click += () => SendMessage("click");
			this.widget.Transfer += () => SendMessage("xfer");
			this.widget.Transfer2 += OnTransfer2;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "chnum")
			{
				widget.Remaining = (int)args[0];
				widget.Available = (int)args[1];
				widget.Built = (int)args[2];
			}
			else
				base.ReceiveMessage(message, args);
		}

		private void OnTransfer2(TransferEventArgs e)
		{
			int mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("xfer2", e.Delta, mods);
		}
	}
}
