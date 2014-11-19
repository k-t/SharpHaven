using System;
using System.Drawing;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class ImageSprite : Sprite
	{
		private TextureSlice tex;
		private Point center;

		public ImageSprite(Resource res)
		{
			Init(res);
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			tex.Draw(batch, x - center.X, y - center.Y, w, h);
		}

		public override void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}

		public bool Init(Resource res)
		{
			var imageData = res.GetLayers<ImageData>();
			if (imageData == null)
				throw new Exception("Sprite resource doesn't contain image data");

			using (var bitmap = ImageUtils.Combine(imageData))
			{
				tex = TextureSlice.FromBitmap(bitmap);
				Width = bitmap.Width;
				Height = bitmap.Height;
			}

			var neg = res.GetLayer<NegData>();
			center = neg != null ? neg.Center : Point.Empty;

			return true;
		}
	}
}
