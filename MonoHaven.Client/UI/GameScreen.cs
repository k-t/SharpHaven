using MonoHaven.Game;

namespace MonoHaven.UI
{
	public class GameScreen : BaseScreen
	{
		private readonly GameSession session;
		private readonly MapView mapView;

		public GameScreen(IScreenHost host)
			: base(host)
		{
			session = new GameSession();
			mapView = new MapView(RootWidget, session);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			mapView.SetSize(newWidth, newHeight);
		}
	}
}
