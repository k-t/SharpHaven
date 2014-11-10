namespace MonoHaven.UI.Remote
{
	public class RemoteContainer : RemoteWidget
	{
		private readonly Container widget;

		public RemoteContainer(int id, RemoteWidget parent) : base(id)
		{
			widget = new Container(parent.Widget);
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
