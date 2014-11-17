using System.Drawing;
using System.IO;
using MonoHaven.Graphics;
using MonoHaven.Resources;

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
					var imageData = res.Value.GetLayer<ImageData>();
					if (imageData == null)
						return null;
					using (var bitmap = new Bitmap(new MemoryStream(imageData.Data)))
						image = new Texture(bitmap);
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
