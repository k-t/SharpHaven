using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class ImageWidget : Widget
	{
		public Texture Image { get; set; }

		protected override void OnDraw(DrawingContext g)
		{
			if (Image == null)
				return;

			g.Draw(Image, 0, 0);
		}
	}
}

