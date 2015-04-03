using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerHud : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Hud(parent.Widget, parent.Session.State);
			return new ServerHud(id, parent, widget);
		}

		private readonly Hud widget;

		public ServerHud(ushort id, ServerWidget parent, Hud widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Menu.ButtonClick += OnMenuButtonClick;
			this.widget.Belt.Activate += (slot) => SendMessage("belt", slot, 1, 0);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "err")
				widget.ShowError((string)args[0]);
			else if (message == "setbelt")
			{
				var slot = (int)args[0];
				var action = args.Length > 1 ? Session.Get<Drawable>((int)args[1]) : null;
				widget.Belt.SetSlot(slot, action);
			}
			else
				base.ReceiveMessage(message, args);
		}

		private void OnMenuButtonClick(HudMenu.Button button)
		{
			string message;
			switch (button)
			{
				case HudMenu.Button.Inventory:
					message = "inv";
					break;
				case HudMenu.Button.Equipment:
					message = "equ";
					break;
				case HudMenu.Button.Character:
					message = "chr";
					break;
				case HudMenu.Button.BuddyList:
					message = "bud";
					break;
				default:
					return;
			}
			SendMessage(message);
		}
	}
}
