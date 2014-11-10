namespace MonoHaven.UI.Remote
{
	public class ContainerController : Controller
	{
		private readonly Container widget;

		private ContainerController(int id, Container widget) : base(id)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, Controller parent, object[] args)
		{
			return new ContainerController(id, new Container(parent.Widget));
		}
	}
}
