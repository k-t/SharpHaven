using System.Drawing;
using SharpHaven.Graphics;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class AimWidget : Widget
	{
		private static readonly Picture bg;
		private static readonly Point bgOffset;
		private static readonly Picture fg;
		private static readonly Point fgOffset;

		static AimWidget()
		{
			var res = App.Resources.Load("ui/aim");
			foreach (var imageData in res.GetLayers<ImageData>())
			{
				if (imageData.Id == 0)
				{
					bg = new Picture(TextureSlice.FromBitmap(imageData.Data), null);
					bgOffset = imageData.Offset;
				}
				if (imageData.Id == 1)
				{
					fg = new Picture(TextureSlice.FromBitmap(imageData.Data), null);
					fgOffset = imageData.Offset;
				}
			}
		}

		public AimWidget(Widget parent) : base(parent)
		{
			Size = bg.Size;
		}

		public int Value
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int i = (10000 - Value) * fg.Height / 10000;
			dc.Draw(bg, bgOffset);
			dc.Draw(fg.Slice(0, i, fg.Width, fg.Height - i), fgOffset.Add(0, i));
		}
	}
}
