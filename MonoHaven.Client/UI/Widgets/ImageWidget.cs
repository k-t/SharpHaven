using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class ImageWidget : Widget
	{
		public Texture Image { get; set; }

		public override void Draw(DrawingContext g)
		{
			if (Image == null)
				return;

			g.Draw(Image, 0, 0);
		}
	}
}

