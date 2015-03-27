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
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "err")
				widget.ShowError((string)args[0]);
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
