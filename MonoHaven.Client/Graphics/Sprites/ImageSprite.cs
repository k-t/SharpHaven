using System;
using System.Drawing;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class ImageSprite : Sprite
	{
		private FutureResource res;
		public Texture tex;
		private Point center;

		public ImageSprite(FutureResource res, byte[] data)
		{
			this.res = res;
			Init();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			// do this only on ticks?
			if (tex == null && !Init())
				return;

			tex.Draw(batch, x - center.X, y - center.Y, w, h);
		}

		public override void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}

		public bool Init()
		{
			if (tex != null || res.Value == null)
				return false;

			var imageData = res.Value.GetLayers<ImageData>();
			if (imageData == null)
				throw new Exception("Sprite resource doesn't contain image data");

			using (var bitmap = ImageUtils.Combine(imageData))
			{
				tex = new Texture(bitmap);
				Width = bitmap.Width;
				Height = bitmap.Height;
			}

			var neg = res.Value.GetLayer<Neg>();
			center = neg != null ? neg.Center : Point.Empty;

			return true;
		}
	}
}
