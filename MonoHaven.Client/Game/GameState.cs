namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly Map map;

		public GameState(GameSession session)
		{
			map = new Map(session);
		}
		
		public Map Map
		{
			get { return map; }
		}
	}
}
