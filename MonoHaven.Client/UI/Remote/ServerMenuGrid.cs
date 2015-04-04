using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		private MenuGrid widget;

		public ServerMenuGrid(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("goto", Goto);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerMenuGrid(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = Session.State.Screen.MenuGrid;
			widget.Actions = Session.State.Actions;
			widget.Visible = true;
			widget.ActionSelected += HandleActionSelected;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
			widget.Actions = null;
		}

		private void Goto(object[] args)
		{
			var resName = (string)args[0];
			widget.SetCurrentAction(resName);
		}

		private void HandleActionSelected(GameAction action)
		{
			SendMessage("act", action.Verbs.ToArray<object>());
		}
	}
}
