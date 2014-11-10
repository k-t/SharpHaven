namespace MonoHaven.UI.Remote
{
	public class ContainerController : Controller
	{
		private readonly Container widget;

		public ContainerController(int id, Controller parent) : base(id)
		{
			widget = new Container(parent.Widget);
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
