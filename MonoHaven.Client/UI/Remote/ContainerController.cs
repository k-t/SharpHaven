using System.Drawing;

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
			var widget = new Container(parent.Widget);
			if (args.Length > 0)
			{
				var size = (Point)args[0];
				widget.SetSize(size.X, size.Y);
			}
			return new ContainerController(id, widget);
		}
	}
}
