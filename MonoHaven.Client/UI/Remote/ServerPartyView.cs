using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerPartyView : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "PARTY";
			return new ServerPartyView(id, parent, widget);
		}

		public ServerPartyView(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
