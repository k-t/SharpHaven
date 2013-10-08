using System;
using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.Widgets
{
	public class Image : Widget
	{
		private Tex image;

		public Image(Point p, Tex image)
			: base(p, image.Size)
		{
			this.image = image;
		}

		public override void Draw(GOut g)
		{
			g.Draw(image, 0, 0);
		}
	}
}

