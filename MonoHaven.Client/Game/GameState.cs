using System.Collections.Generic;
using MonoHaven.Network.Messages;

namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly GameActionTree actions;
		private readonly Dictionary<string, CharAttribute> attributes;
		private readonly Dictionary<int, BuffData> buffs;
		private readonly Map map;
		private readonly GobCache objects;
		private readonly GameScene scene;

		public GameState(GameSession session)
		{
			actions = new GameActionTree();
			attributes = new Dictionary<string, CharAttribute>();
			buffs = new Dictionary<int, BuffData>();
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

		public Astonomy Astronomy
		{
			get;
			set;
		}

		public CharAttribute GetAttr(string name)
		{
			CharAttribute attr;
			return attributes.TryGetValue(name, out attr) ? attr : null;
		}

		public void SetAttr(CharAttribute attr)
		{
			CharAttribute old;
			if (attributes.TryGetValue(attr.Name, out old))
				old.Update(attr.BaseValue, attr.ComputedValue);
			else
				attributes[attr.Name] = attr;
		}

		public void AddBuff(BuffData buff)
		{
			buffs[buff.Id] = buff;
		}

		public void ClearBuffs()
		{
			buffs.Clear();
		}

		public IEnumerable<BuffData> GetBuffs()
		{
			return buffs.Values;
		}

		public void RemoveBuff(int id)
		{
			buffs.Remove(id);
		}
	}
}
