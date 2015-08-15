using SharpHaven.Graphics;

namespace SharpHaven.UI
{
	public class Drag
	{
		public Drag(Drawable image, object tag)
		{
			Image = image;
			Tag = tag;
		}

		public Drawable Image { get; }

		public object Tag { get; }
	}
}
