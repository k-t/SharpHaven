using System.Linq;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerAvatarView : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			int gobId = (int)args[0];
			var widget = new AvatarView(parent.Widget);
			widget.Avatar = new Avatar(gobId, parent.Session.State.Objects);
			return new ServerAvatarView(id, parent, widget);
		}

		public static ServerWidget CreateFromLayers(ushort id, ServerWidget parent, object[] args)
		{
			var session = parent.Session;
			var layers = args.Select(x => session.GetSprite((int)x));
			var widget = new AvatarView(parent.Widget);
			widget.Avatar = new Avatar(layers);
			return new ServerAvatarView(id, parent, widget);
		}

		public ServerAvatarView(ushort id, ServerWidget parent, AvatarView widget)
			: base(id, parent, widget)
		{
		}
	}
}
