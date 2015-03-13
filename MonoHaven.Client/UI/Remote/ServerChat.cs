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

		public ServerChat(ushort id, ServerWidget parent, Chat widget)
			: base(id, parent, widget)
		{
		}
	}
}
