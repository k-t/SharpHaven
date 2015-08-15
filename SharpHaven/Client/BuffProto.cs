using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class BuffProto
	{
		public BuffProto(Drawable image, string tooltip)
		{
			Image = image;
			Tooltip = tooltip;
		}

		public Drawable Image { get; }

		public string Tooltip { get; }
	}
}
