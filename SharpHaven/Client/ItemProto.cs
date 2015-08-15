using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class ItemProto
	{
		public ItemProto(Drawable image, string tooltip)
		{
			Image = image;
			Tooltip = tooltip;
		}

		public Drawable Image { get; }

		public string Tooltip { get; }
	}
}
