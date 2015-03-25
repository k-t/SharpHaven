using MonoHaven.Graphics;

namespace MonoHaven.Game
{
	public class BuffMold
	{
		private readonly Drawable image;
		private readonly string tooltip;

		public BuffMold(Drawable image, string tooltip)
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
