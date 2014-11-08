using MonoHaven.UI;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private readonly MapView mapView;

		public GameScreen(GameState gstate)
		{
			this.mapView = new MapView(RootWidget, gstate);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			mapView.SetSize(newWidth, newHeight);
		}
	}
}
