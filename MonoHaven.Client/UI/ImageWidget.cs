using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class ImageWidget : Widget
	{
		public Drawable Image { get; set; }

		protected override void OnDraw(DrawingContext dc)
		{
			if (Image == null)
				return;

			dc.Draw(Image, 0, 0);
		}
	}
}

