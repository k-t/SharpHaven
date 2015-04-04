using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerGobAvatarView : ServerWidget
	{
		private AvatarView widget;

		public ServerGobAvatarView(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerGobAvatarView(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			int gobId = (int)args[0];
			
			widget = new AvatarView(Parent.Widget);
			widget.Move(position);
			widget.Avatar = new Avatar(gobId, Session.State.Objects);
		}
	}
}
