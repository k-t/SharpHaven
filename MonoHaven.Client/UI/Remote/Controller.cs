using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class Controller
	{
		private readonly ushort id;
		private readonly GameSession session;
		private readonly WidgetAdapter adapter;
		private readonly Widget widget;

		public Controller(ushort id, GameSession session, Widget widget, WidgetAdapter adapter)
		{
			this.id = id;
			this.widget = widget;
			this.adapter = adapter;
			this.session = session;
			
			adapter.SetEventHandler(widget, HandleWidgetMessage);
		}

		public Widget Widget
		{
			get { return widget; }
		}

		public void HandleRemoteMessage(string message, object[] args)
		{
			adapter.HandleMessage(widget, message, args);
		}

		private void HandleWidgetMessage(string message, object[] args)
		{
			session.SendWidgetMessage(id, message, args);
		}
	}
}
