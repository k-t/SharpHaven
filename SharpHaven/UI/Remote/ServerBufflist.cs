using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerBufflist : ServerWidget
	{
		private Bufflist widget;

		public ServerBufflist(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerBufflist(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = new Bufflist(Parent.Widget, Parent.Session);
			widget.Move(position);
		}
	}
}
