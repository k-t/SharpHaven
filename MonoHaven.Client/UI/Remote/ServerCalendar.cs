using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerCalendar : ServerWidget
	{
		private Widget widget;

		public ServerCalendar(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCalendar(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			widget = new Calendar(Parent.Widget, Parent.Session.State);
		}
	}
}
