using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MonoHaven.Resources;

namespace MonoHaven.Utils
{
	public class ImageUtils
	{
		public static Bitmap Combine(IEnumerable<ImageData> images)
		{
			var bitmaps = new List<Bitmap>();
			try
			{
				int w = 0;
				int h = 0;
				foreach (var image in images)
				{
					var bitmap = new Bitmap(new MemoryStream(image.Data));
					w = Math.Max(w, bitmap.Width + image.OffsetX);
					h = Math.Max(h, bitmap.Height + image.OffsetY);
					bitmaps.Add(bitmap);
				}
				var combined = new Bitmap(w + 1, h + 1);
				using (var g = System.Drawing.Graphics.FromImage(combined))
				{
					g.Clear(Color.Transparent);
					foreach (var bitmap in bitmaps)
						g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
				}
				return combined;
			}
			finally
			{
				foreach (var bitmap in bitmaps)
					bitmap.Dispose();
			}
		}
	}
}
