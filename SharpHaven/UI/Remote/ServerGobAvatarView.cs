using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
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

		protected override void OnInit(Coord2D position, object[] args)
		{
			int gobId = (int)args[0];
			
			widget = new AvatarView(Parent.Widget);
			widget.Move(position);
			widget.Avatar = new Avatar(gobId, Session.Objects);
		}
	}
}
