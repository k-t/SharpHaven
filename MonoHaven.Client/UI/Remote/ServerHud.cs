using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerHud : ServerWidget
	{
		private Hud widget;

		public ServerHud(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("err", args => widget.ShowError((string)args[0]));
			SetHandler("setbelt", SetBelt);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerHud(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			widget = new Hud(Parent.Widget, Parent.Session.State);
			widget.Menu.ButtonClick += OnMenuButtonClick;
			widget.Belt.Activate += (slot) => SendMessage("belt", slot, 1, 0);
		}

		private void SetBelt(object[] args)
		{
			var slot = (int)args[0];
			var action = args.Length > 1 ? Session.Get<Drawable>((int)args[1]) : null;
			widget.Belt.SetSlot(slot, action);
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
