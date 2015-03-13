using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerRootWidget : ServerWidget
	{
		public ServerRootWidget(ushort id, GameSession session, Widget widget)
			: base(id, session, widget)
		{
		}
	}
}
