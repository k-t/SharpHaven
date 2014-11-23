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
			missing = App.Instance.Resources.GetTexture("gfx/invobjs/missing");
		}

		private readonly Delayed<Resource> res;
		private Drawable image;

		public ItemWidget(Widget parent, Delayed<Resource> res) : base(parent)
		{
			this.res = res;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (image == null && res.Value == null)
				dc.Draw(missing, 0, 0, defaultSize.X, defaultSize.Y);
			else
			{
				if (image == null)
				{
					image = TextureSlice.FromBitmap(res.Value.GetLayer<ImageData>().Data);
					FixSize();
				}
				dc.Draw(image, 0, 0);
			}
			
		}

		protected override void OnDispose()
		{
			if (image != null)
				image.Dispose();
		}

		private void FixSize()
		{
			SetSize(image.Width, image.Height);
		}
	}
}
