using System;
using System.Collections.Generic;

namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly GameActionTree actions;
		private readonly Dictionary<string, CharAttribute> attributes;
		private readonly Dictionary<int, Buff> buffs;
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;
		private readonly Party party;

		public GameState(GameSession session)
		{
			actions = new GameActionTree();
			attributes = new Dictionary<string, CharAttribute>();
			buffs = new Dictionary<int, Buff>();
			map = new Map(session);
			objects = new GobCache();
			scene = new GameScene(this);
			party = new Party();
		}

		public event Action BuffUpdated;
		
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

		public Party Party
		{
			get { return party; }
		}

		public IEnumerable<Buff> Buffs
		{
			get { return buffs.Values; }
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

		public void UpdateBuff(Buff buff)
		{
			buffs[buff.Id] = buff;
			BuffUpdated.Raise();
		}

		public void RemoveBuff(int id)
		{
			buffs.Remove(id);
			BuffUpdated.Raise();
		}

		public void ClearBuffs()
		{
			buffs.Clear();
			BuffUpdated.Raise();
		}
	}
}
