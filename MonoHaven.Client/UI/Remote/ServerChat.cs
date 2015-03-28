using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerChat : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var title = (string)args[0];
			var closable = (args.Length > 1) && ((int)args[1]) != 0;

			var widget = new Chat(parent.Widget, title, closable);
			return new ServerChat(id, parent, widget);
		}

		private readonly Chat widget;

		public ServerChat(ushort id, ServerWidget parent, Chat widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "log")
			{
				var text = (string)args[0];
				var color = args.Length > 1 ? (Color?)args[1] : null;
				var alert = args.Length > 2 ? (Color?)args[2] : null;
				widget.AddMessage(text, color);
			}
			// TODO:
			//else if (message == "focusme")
			//{
			//}
			else
				base.ReceiveMessage(message, args);
		}
	}
}
