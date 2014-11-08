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
			connection.Closed += OnConnectionClosed;
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

		public void Finish()
		{
			connection.Closed -= OnConnectionClosed;
			connection.Close();
			screen.Close();
		}

		private void OnConnectionClosed()
		{
			Finish();
		}
	}
}
