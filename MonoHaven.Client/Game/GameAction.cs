using System;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GameAction : TreeNode<GameAction>, IComparable<GameAction>
	{
		private readonly GameActionInfo info;

		public GameAction()
		{
		}

		public GameAction(GameActionInfo info)
		{
			this.info = info;
		}

		public string Name
		{
			get { return info.Name; }
		}

		public string Tooltip
		{
			get { return info.Tooltip; }
		}

		public Drawable Image
		{
			get { return info.Image; }
		}
		
		public string[] Verbs
		{
			get { return info.Verbs; }
		}

		public int CompareTo(GameAction other)
		{
			if (other == null) return 1;
			if (other.HasChildren != HasChildren)
				return other.HasChildren ? 1 : -1;
			return string.CompareOrdinal(Name, other.Name);
		}
	}
}
