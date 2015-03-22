using MonoHaven.Graphics;

namespace MonoHaven.Game
{
	public class ItemMold
	{
		private readonly Drawable image;
		private readonly string tooltip;

		public ItemMold(Drawable image, string tooltip)
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
