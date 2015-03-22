using System;
using C5;

namespace MonoHaven.Game
{
	public class GameActionTree
	{
		private readonly GameAction root;
		private readonly HashDictionary<string, GameAction> actions;

		public GameActionTree()
		{
			root = new GameAction();
			actions = new HashDictionary<string, GameAction>();
		}

		public event Action Changed;

		public GameAction Root
		{
			get { return root; }
		}

		public void Add(string resName)
		{
			if (actions.Contains(resName))
				return;
			actions.Add(resName, Load(resName));
			Changed.Raise();
		}

		public GameAction Get(string resName)
		{
			GameAction act;
			return actions.Find(ref resName, out act) ? act : null;
		}

		public void Remove(string resName)
		{
			if (actions.Remove(resName))
				Changed.Raise();
		}

		private GameAction GetOrAdd(string resName)
		{
			if (string.IsNullOrEmpty(resName))
				return null;
			GameAction act;
			if (!actions.Find(ref resName, out act))
			{
				act = Load(resName);
				actions[resName] = act;
			}
			return act;
		}

		private GameAction Load(string resName)
		{
			var info = App.Resources.Get<GameActionInfo>(resName);
			var act = new GameAction(info);
			var parent = GetOrAdd(info.Parent.Name) ?? root;
			parent.AddChild(act);
			return act;
		}
	}
}
