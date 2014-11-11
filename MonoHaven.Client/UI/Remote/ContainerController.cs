using System.Drawing;
using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class ContainerController : Controller
	{
		private readonly Container widget;

		private ContainerController(int id, GameSession session, Container widget)
			: base(id, session)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, GameSession session, Controller parent, object[] args)
		{
			var widget = new Container(parent.Widget);
			if (args.Length > 0)
			{
				var size = (Point)args[0];
				widget.SetSize(size.X, size.Y);
			}
			return new ContainerController(id, session, widget);
		}
	}
}
