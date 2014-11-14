using System.Linq;

namespace MonoHaven.UI.Remote
{
	public class ServerAvatarView : ServerWidget
	{
		public static ServerWidget CreateFromLayers(ushort id, ServerWidget parent, object[] args)
		{
			var session = parent.Session;
			var layers = args.Select(x => session.GetResource((int)x));
			var widget = new AvatarView(parent.Widget, layers);
			return new ServerAvatarView(id, parent, widget);
		}

		public ServerAvatarView(ushort id, ServerWidget parent, AvatarView widget)
			: base(id, parent, widget)
		{
		}
	}
}
