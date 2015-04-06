using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class GameAction
	{
		private readonly string name;
		private readonly string resName;
		private readonly ResourceRef parent;
		private readonly string tooltip;
		private readonly Drawable image;
		private readonly string[] verbs;

		public GameAction()
		{
		}

		public GameAction(
			string name,
			string resName,
			ResourceRef parent,
			string tooltip,
			Drawable image,
			string[] verbs)
		{
			this.name = name;
			this.resName = resName;
			this.tooltip = tooltip;
			this.image = image;
			this.verbs = verbs;
			this.parent = parent;
		}

		public string Name
		{
			get { return name; }
		}

		public string ResName
		{
			get { return resName; }
		}

		public ResourceRef Parent
		{
			get { return parent; }
		}

		public bool HasParent
		{
			get { return !string.IsNullOrEmpty(parent.Name); }
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
	}
}
