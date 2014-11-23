using MonoHaven.Game;

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
