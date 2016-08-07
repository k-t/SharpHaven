using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerRootWidget : ServerWidget
	{
		private readonly Widget widget;

		public ServerRootWidget(ushort id, ClientSession session, Widget widget)
			: base(id, session)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		protected override void OnInit(Coord2D position, object[] args)
		{
		}
	}
}
