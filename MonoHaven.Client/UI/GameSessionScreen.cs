using MonoHaven.Game;

namespace MonoHaven.UI
{
	public class GameSessionScreen : BaseScreen
	{
		private readonly GameSession session;
		private readonly MapView mapView;

		public GameSessionScreen(IScreenHost host)
			: base(host)
		{
			session = new GameSession();

			mapView = new MapView(RootWidget, session);
			mapView.SetSize(host.Width, host.Height);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			mapView.SetSize(newWidth, newHeight);
		}
	}
}
