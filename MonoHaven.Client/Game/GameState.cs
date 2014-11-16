namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly Map map;
		private readonly GameObjectCache objects;

		public GameState(GameSession session)
		{
			map = new Map(session);
			objects = new GameObjectCache();
		}
		
		public Map Map
		{
			get { return map; }
		}

		public GameObjectCache Objects
		{
			get { return objects; }
		}
	}
}
