using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class RootController : Controller
	{
		private readonly RootWidget widget;

		public RootController(int id, GameSession session, RootWidget widget)
			: base(id, session)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
