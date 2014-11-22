#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
