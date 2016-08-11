using Haven;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerWindow : ServerWidget
	{
		private Window widget;

		public ServerWindow(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("pack", Pack);
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerWindow(id, parent);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		protected override void OnInit(Point2D position, object[] args)
		{
			var size = (Point2D)args[0];
			var caption = args.Length > 1 ? (string)args[1] : "";

			widget = new Window(Parent.Widget, caption);
			widget.Move(position);
			widget.Resize(size.X, size.Y);
			widget.Closed += () => SendMessage("close");
		}

		private void Pack(object[] args)
		{
			widget.Pack();
		}
	}
}
