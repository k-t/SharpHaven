using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class Skill
	{
		public Skill(string id, Drawable image, string tooltip)
		{
			Id = id;
			Image = image;
			Tooltip = tooltip;
		}

		public string Id { get; }

		public Drawable Image { get; }

		public string Tooltip { get; }

		public int? Cost { get; set; }
	}
}
