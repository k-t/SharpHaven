namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly GameActionTree actions;
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;

		public GameState(GameSession session)
		{
			actions = new GameActionTree();
			map = new Map(session);
			objects = new GobCache();
			scene = new GameScene(this);
		}
		
		public Map Map
		{
			get { return map; }
		}

		public GameActionTree Actions
		{
			get { return actions; }
		}

		public GobCache Objects
		{
			get { return objects; }
		}

		public GameScene Scene
		{
			get { return scene; }
		}

		public GameTime Time
		{
			get;
			set;
		}
	}
}
