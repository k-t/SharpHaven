namespace MonoHaven.UI.Remote
{
	public class RootController : Controller
	{
		private readonly RootWidget widget;

		public RootController(int id, RootWidget rootWidget) : base(id)
		{
			widget = rootWidget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
