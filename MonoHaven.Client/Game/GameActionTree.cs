using System;
using System.Collections.Generic;
using System.Linq;
using C5;

namespace MonoHaven.Game
{
	using GameActionNode = Utils.ValueTreeNode<GameAction>;

	public class GameActionTree
	{
		private readonly GameActionNode root;
		private readonly HashDictionary<string, GameActionNode> nodes;

		public GameActionTree()
		{
			root = new GameActionNode(new GameAction());
			nodes = new HashDictionary<string, GameActionNode>();
		}

		public event Action Changed;

		public GameAction Root
		{
			get { return root.Value; }
		}

		public GameAction GetByName(string resName)
		{
			GameActionNode node;
			return nodes.Find(ref resName, out node) ? node.Value : null;
		}

		public bool HasChildren(GameAction action)
		{
			return action != Root
				? nodes.Values.First(x => x.Value == action).HasChildren
				: root.HasChildren;
		}

		public IEnumerable<GameAction> GetChildren(GameAction action)
		{
			return action != Root
				? nodes.Values.First(x => x.Value == action).Children.Select(x => x.Value)
				: root.Children.Select(x => x.Value);
		}

		public void Add(string resName)
		{
			if (!nodes.Contains(resName))
			{
				nodes.Add(resName, Load(resName));
				Changed.Raise();
			}
		}

		public void Remove(string resName)
		{
			if (nodes.Remove(resName))
				Changed.Raise();
		}

		private GameActionNode Load(string resName)
		{
			var action = App.Resources.Get<GameAction>(resName);
			var node = new GameActionNode(action);
			var parent = GetOrLoad(action.Parent.Name) ?? root;
			parent.AddChild(node);
			return node;
		}

		private GameActionNode GetOrLoad(string resName)
		{
			if (string.IsNullOrEmpty(resName))
				return null;

			GameActionNode node;
			if (!nodes.Find(ref resName, out node))
			{
				node = Load(resName);
				nodes[resName] = node;
			}
			return node;
		}
	}
}
