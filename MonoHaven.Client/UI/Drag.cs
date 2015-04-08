using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Drag
	{
		private readonly Drawable image;
		private readonly object tag;

		public Drag(Drawable image, object tag)
		{
			this.image = image;
			this.tag = tag;
		}

		public Drawable Image
		{
			get { return image; }
		}

		public object Tag
		{
			get { return tag; }
		}
	}
}
