using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerGiveButton : ServerWidget
	{
		private GiveButton widget;

		public ServerGiveButton(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerGiveButton(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			widget = new GiveButton(Parent.Widget);
		}
	}
}
