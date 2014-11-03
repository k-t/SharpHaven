using MonoHaven.Game;

namespace MonoHaven.UI
{
	public class GameScreen : BaseScreen
	{
		private readonly MapView mapView;

		public GameScreen(IScreenHost host, GameState gstate)
			: base(host)
		{
			this.mapView = new MapView(RootWidget, gstate);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			mapView.SetSize(newWidth, newHeight);
		}
	}
}
