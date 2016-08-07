using System.Collections;
using System.Drawing;
using System.IO;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class DrawableFactory : IObjectFactory<Drawable>
	{
		public Drawable Create(string resName, Resource res)
		{
			var imageData = res.GetLayer<ImageLayer>();
			if (imageData == null)
				return null;

			using (var ms = new MemoryStream(imageData.Data))
			using (var bitmap = new Bitmap(ms))
			{
				// load texture
				var tex = TextureSlice.FromBitmap(bitmap);
				// check whether image is a ninepatch
				var ninepatch = res.GetLayer<NinepatchLayer>();
				if (ninepatch != null)
				{
					return new NinePatch(tex, ninepatch.Left, ninepatch.Right,
						ninepatch.Top, ninepatch.Bottom);
				}
				return new Picture(tex, GenerateHitmask(bitmap));
			}
		}

		private BitArray GenerateHitmask(Bitmap bitmap)
		{
			// TODO: same thing exists for sprite
			var hitmask = new BitArray(bitmap.Width * bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
				{
					var pixel = bitmap.GetPixel(i, j);
					hitmask[i * bitmap.Height + j] = pixel.A > 128;
				}
			return hitmask;
		}
	}
}
