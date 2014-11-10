namespace MonoHaven.UI.Remote
{
	public class ContainerController : Controller
	{
		private readonly Container widget;

		private ContainerController(int id, Controller parent) : base(id)
		{
			widget = new Container(parent.Widget);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, Controller parent, object[] args)
		{
			return new ContainerController(id, parent);
		}
	}
}
