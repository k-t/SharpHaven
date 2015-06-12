using System.Drawing;
using SharpHaven.Client;
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

		protected override void OnInit(Point position, object[] args)
		{
		}
	}
}
