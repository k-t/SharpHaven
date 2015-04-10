using System.Drawing;
using System.IO;
using OpenTK;

namespace SharpHaven.Resources
{
	public class MouseCursorFactory : IObjectFactory<MouseCursor>
	{
		public MouseCursor Create(string resName, Resource res)
		{
			var imageData = res.GetLayer<ImageData>();
			if (imageData == null)
				return null;
			
			using (var ms = new MemoryStream(imageData.Data))
			using (var bitmap = new Bitmap(ms))
			{
				var bitmapData = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var cursor = new MouseCursor(1, 1, bitmap.Width, bitmap.Height, bitmapData.Scan0);
				bitmap.UnlockBits(bitmapData);
				return cursor;
			}
		}
	}
}
