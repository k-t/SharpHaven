using System;
using C5;
using MonoHaven.Graphics;
using MonoHaven.Resources;

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
			var res = App.Resources.Get(resName);
			var data = res.GetLayer<ActionData>();
			// FIXME: that should be moved to resource manager!
			var image = TextureSlice.FromBitmap(res.GetLayer<ImageData>().Data);
			var act = new GameAction(data.Name, data.Name, new Picture(image, null), data.Verbs);

			var parent = GetOrAdd(data.Parent.Name) ?? root;
			parent.AddChild(act);

			return act;
		}
	}
}
