using System.Drawing;
using System.Linq;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerFlowerMenu : ServerWidget
	{
		private FlowerMenu widget;

		public ServerFlowerMenu(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("act", _ => widget.Remove());
			SetHandler("cancel", _ => widget.Remove());
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerFlowerMenu(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var options = args.OfType<string>();
			
			widget = new FlowerMenu(Parent.Widget, options);
			widget.Selected += n => SendMessage("cl", n);

			if (position.X != -1 && position.Y != -1)
				widget.Move(position);
			else
				widget.Move(Session.State.Screen.MousePosition);
		}
	}
}
