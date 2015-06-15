using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class ItemProto
	{
		private readonly Drawable image;
		private readonly string tooltip;

		public ItemProto(Drawable image, string tooltip)
		{
			this.image = image;
			this.tooltip = tooltip;
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
