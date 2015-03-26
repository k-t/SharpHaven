using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerGiveButton : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new GiveButton(parent.Widget);
			return new ServerGiveButton(id, parent, widget);
		}

		private readonly GiveButton widget;

		public ServerGiveButton(ushort id, ServerWidget parent, GiveButton widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}
	}
}
