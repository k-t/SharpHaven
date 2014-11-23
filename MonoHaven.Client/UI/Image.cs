using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Image : Widget
	{
		public Image(Widget parent) : base(parent)
		{
		}

		public Drawable Drawable
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Drawable == null)
				return;

			dc.Draw(Drawable, 0, 0);
		}
	}
}

