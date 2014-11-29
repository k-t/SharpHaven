using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerWindow : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var caption = args.Length > 1 ? (string)args[1] : "";

			var window = new Window(parent.Widget, caption);
			window.Resize(size.X, size.Y);
			return new ServerWindow(id, parent, window);
		}

		private readonly Window widget;

		public ServerWindow(ushort id, ServerWidget parent, Window widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Closed += () => SendMessage("close");
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "pack")
				widget.Pack();
			else
				base.ReceiveMessage(message, args);
		}
	}
}
