using MonoHaven.Login;
using MonoHaven.UI;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private readonly MapView mapView;
		private readonly GameState gstate;

		public GameScreen(GameState gstate)
		{
			this.gstate = gstate;
			this.mapView = new MapView(RootWidget, gstate);
		}

		public void Close()
		{
			Host.SetScreen(new LoginScreen());
		}

		protected override void OnShow()
		{
			gstate.LoadMap();
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			mapView.SetSize(newWidth, newHeight);
		}
	}
}
