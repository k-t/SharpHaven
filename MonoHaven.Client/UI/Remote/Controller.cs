namespace MonoHaven.UI.Remote
{
	public class Controller
	{
		private readonly int id;
		private readonly WidgetAdapter adapter;
		private readonly Widget widget;

		public Controller(int id, Widget widget, WidgetAdapter adapter)
		{
			this.id = id;
			this.widget = widget;
			this.adapter = adapter;
			
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
		}
	}
}
