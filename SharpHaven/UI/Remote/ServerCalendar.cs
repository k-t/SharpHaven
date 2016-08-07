using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerCalendar : ServerWidget
	{
		private Calendar widget;

		public ServerCalendar(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return null; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCalendar(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			widget = Session.Screen.Calendar;
			widget.Visible = true;
			widget.Session = Session;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
			widget.Session = null;
		}
	}
}
