namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;

		public GameState(GameSession session)
		{
			map = new Map(session);
			objects = new GobCache();
			scene = new GameScene(this);
		}
		
		public Map Map
		{
			get { return map; }
		}

		public GobCache Objects
		{
			get { return objects; }
		}

		public GameScene Scene
		{
			get { return scene; }
		}
	}
}
