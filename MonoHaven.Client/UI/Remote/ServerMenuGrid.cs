using System.Linq;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new MenuGrid(parent.Widget, parent.Session.State.Actions);
			return new ServerMenuGrid(id, parent, widget);
		}

		private readonly MenuGrid widget;

		public ServerMenuGrid(ushort id, ServerWidget parent, MenuGrid widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.ActionSelected += HandleActionSelected;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "goto")
			{
				var resName = (string)args[0];
				widget.SetCurrentAction(resName);
			}
			else
				base.ReceiveMessage(message, args);
		}

		private void HandleActionSelected(GameAction action)
		{
			SendMessage("act", action.Verbs.ToArray<object>());
		}
	}
}
