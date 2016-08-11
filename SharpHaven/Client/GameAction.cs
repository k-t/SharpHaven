using Haven.Resources;
using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class GameAction
	{
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
			Name = name;
			ResName = resName;
			Tooltip = tooltip;
			Image = image;
			Verbs = verbs;
			Parent = parent;
		}

		public string Name { get; }

		public string ResName { get; }

		public ResourceRef Parent { get; }

		public string Tooltip { get; }

		public Drawable Image { get; }

		public string[] Verbs { get; }

		public bool HasParent
		{
			get { return !string.IsNullOrEmpty(Parent.Name); }
		}
	}
}
