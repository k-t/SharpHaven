using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class ItemWidget : Widget
	{
		private static readonly Drawable missing;
		private static readonly Point defaultSize = new Point(30, 30);

		static ItemWidget()
		{
			missing = App.Resources.GetImage("gfx/invobjs/missing");
		}

		private readonly Delayed<Drawable> image;
		private bool isSizeFixed;

		public ItemWidget(Widget parent, Delayed<Drawable> image)
			: base(parent)
		{
			this.image = image;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (image.Value == null)
				dc.Draw(missing, 0, 0, defaultSize.X, defaultSize.Y);
			else
			{
				if (!isSizeFixed)
					FixSize();
				dc.Draw(image.Value, 0, 0);
			}
			
		}

		protected override void OnDispose()
		{
			if (image.Value != null)
				image.Value.Dispose();
		}

		private void FixSize()
		{
			SetSize(image.Value.Width, image.Value.Height);
			isSizeFixed = true;
		}
	}
}
