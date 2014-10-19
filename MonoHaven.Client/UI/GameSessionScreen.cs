using MonoHaven.Game;
using MonoHaven.UI.Widgets;

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

			this.mapView = new MapView(session) { Width = host.Width, Height = host.Height };
			this.Add(mapView);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			this.mapView.Width = Host.Width;
			this.mapView.Height = Host.Height;
		}
	}
}
