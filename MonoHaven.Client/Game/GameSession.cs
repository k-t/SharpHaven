using MonoHaven.Network;
using MonoHaven.UI;

namespace MonoHaven.Game
{
	public class GameSession
	{
		private readonly Connection connection;
		private readonly GameState state;
		private readonly GameScreen screen;

		public GameSession(ConnectionSettings connSettings)
		{
			connection = new Connection(connSettings);
			state = new GameState();
			screen = new GameScreen(state);
		}

		public IScreen Screen
		{
			get { return screen; }
		}

		public void Start()
		{
			connection.Open();
		}
	}
}
