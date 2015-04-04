using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerWindow : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerWindow(id, parent);
		}

		private Window widget;

		public ServerWindow(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("pack", Pack);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		protected override void OnInit(object[] args)
		{
			var size = (Point)args[0];
			var caption = args.Length > 1 ? (string)args[1] : "";

			widget = new Window(Parent.Widget, caption);
			widget.Resize(size.X, size.Y);
			widget.Closed += () => SendMessage("close");
		}

		private void Pack(object[] args)
		{
			widget.Pack();
		}
	}
}
