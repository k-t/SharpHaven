namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly Map map;
		private readonly GobCache objects;

		public GameState(GameSession session)
		{
			map = new Map(session);
			objects = new GobCache();
		}
		
		public Map Map
		{
			get { return map; }
		}

		public GobCache Objects
		{
			get { return objects; }
		}
	}
}
