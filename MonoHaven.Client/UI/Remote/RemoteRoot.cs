namespace MonoHaven.UI.Remote
{
	public class RemoteRoot : RemoteWidget
	{
		private readonly RootWidget widget;

		public RemoteRoot(int id, RootWidget rootWidget) : base(id)
		{
			widget = rootWidget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
