using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class RootController : Controller
	{
		private readonly RootWidget widget;

		public RootController(int id, RootWidget widget) : base(id)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
