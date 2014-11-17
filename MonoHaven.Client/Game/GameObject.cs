using System.Drawing;
using System.IO;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private FutureResource res;
		private Drawable image; 

		public Point Position
		{
			get;
			set;
		}

		public Point DrawOffset
		{
			get;
			set;
		}

		public Drawable Image
		{
			get
			{
				if (image == null && res != null && res.Value != null)
				{
					var imageData = res.Value.GetLayers<ImageData>();
					if (imageData == null)
						return null;
					var neg = res.Value.GetLayer<Neg>();
					var negc = neg != null ? neg.Center : Point.Empty;
					using (var bitmap = ImageUtils.Combine(imageData))
					{
						if (image != null)
							image.Dispose();
						image = new Texture(bitmap);
						image.DrawOffset = new Point(-negc.X, -negc.Y);
					}
				}
				return image;
			}
		}

		public void SetResource(FutureResource value)
		{
			res = value;
		}
	}
}
