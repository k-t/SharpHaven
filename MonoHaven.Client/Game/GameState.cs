using System.Collections.Generic;
using MonoHaven.Messages;

namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly GameActionTree actions;
		private readonly Dictionary<string, CharAttribute> attributes;
		private readonly Dictionary<int, BuffAddMessage> buffs;
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;

		public GameState(GameSession session)
		{
			actions = new GameActionTree();
			attributes = new Dictionary<string, CharAttribute>();
			buffs = new Dictionary<int, BuffAddMessage>();
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

		public CharAttribute GetAttr(string name)
		{
			CharAttribute attr;
			return attributes.TryGetValue(name, out attr) ? attr : null;
		}

		public void SetAttr(string name, int baseValue, int compValue)
		{
			CharAttribute attr;
			if (attributes.TryGetValue(name, out attr))
				attr.Update(baseValue, compValue);
			else
			{
				attr = new CharAttribute(name, baseValue, compValue);
				attributes[name] = attr;
			}
		}

		public void AddBuff(BuffAddMessage buff)
		{
			buffs[buff.Id] = buff;
		}

		public void ClearBuffs()
		{
			buffs.Clear();
		}

		public IEnumerable<BuffAddMessage> GetBuffs()
		{
			return buffs.Values;
		}

		public void RemoveBuff(int id)
		{
			buffs.Remove(id);
		}
	}
}
