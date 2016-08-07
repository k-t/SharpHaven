using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerContainer : ServerWidget
	{
		private Container widget;

		public ServerContainer(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public ServerContainer(ushort id, ServerWidget parent, Container widget)
			: base(id, parent)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerContainer(id, parent);
		}

		protected override void OnInit(Coord2D position, object[] args)
		{
			var size = (Coord2D)args[0];

			widget = Session.Screen.Container;
			widget.Resize(size.X, size.Y);
			widget.Visible = true;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
		}
	}
}
