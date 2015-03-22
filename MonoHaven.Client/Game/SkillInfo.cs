using MonoHaven.Graphics;

namespace MonoHaven.Game
{
	public class SkillInfo
	{
		private readonly string id;
		private readonly Drawable image;
		private readonly string tooltip;

		public SkillInfo(string id, Drawable image, string tooltip)
		{
			this.id = id;
			this.image = image;
			this.tooltip = tooltip;
		}

		public string Id
		{
			get { return id; }
		}

		public Drawable Image
		{
			get { return image; }
		}

		public string Tooltip
		{
			get { return tooltip; }
		}
	}
}
