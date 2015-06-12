using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class Skill
	{
		private readonly string id;
		private readonly Drawable image;
		private readonly string tooltip;

		public Skill(string id, Drawable image, string tooltip)
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

		public int? Cost
		{
			get;
			set;
		}
	}
}
