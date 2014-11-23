using System;
using C5;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using MonoHaven.Resources.Layers;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GameActionTree : IDisposable
	{
		private readonly GameAction root;
		private readonly HashDictionary<string, GameAction> actions;
		// TODO: atlas should be managed somewhere else
		private readonly TextureAtlas atlas;

		public GameActionTree()
		{
			root = new GameAction();
			atlas = new TextureAtlas(1024, 1024);
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

		public void Dispose()
		{
			atlas.Dispose();
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
			var res = App.Instance.Resources.Get(resName);
			var data = res.GetLayer<ActionData>();
			var image =  atlas.Add(res.GetLayer<ImageData>().Data);
			var act = new GameAction(data.Name, data.Name, image, data.Verbs);

			var parent = GetOrAdd(data.Parent.Name) ?? root;
			parent.AddChild(act);

			return act;
		}
	}
}
