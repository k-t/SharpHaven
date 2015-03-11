using System;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GameAction : TreeNode<GameAction>, IComparable<GameAction>
	{
		private readonly string name;
		private readonly string tooltip;
		private readonly Drawable image;
		private readonly string[] verbs;

		public GameAction()
		{
		}

		public GameAction(
			string name,
			string tooltip,
			Drawable image,
			string[] verbs)
		{
			this.name = name;
			this.tooltip = tooltip;
			this.image = image;
			this.verbs = verbs;
		}

		public string Name
		{
			get { return name; }
		}

		public string Tooltip
		{
			get { return tooltip; }
		}

		public Drawable Image
		{
			get { return image; }
		}
		
		public string[] Verbs
		{
			get { return verbs; }
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
