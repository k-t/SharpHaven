using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Utils
{
	public class ImageUtils
	{
		public static Bitmap Combine(IEnumerable<ImageData> images)
		{
			var bitmaps = new List<Tuple<Bitmap, Point>>();
			try
			{
				int w = 0;
				int h = 0;
				foreach (var image in images.OrderBy(x => x.Z))
				{
					var bitmap = new Bitmap(new MemoryStream(image.Data));
					w = Math.Max(w, bitmap.Width + image.DrawOffset.X);
					h = Math.Max(h, bitmap.Height + image.DrawOffset.Y);
					bitmaps.Add(Tuple.Create(bitmap, image.DrawOffset));
				}
				var combined = new Bitmap(w + 1, h + 1);
				using (var g = System.Drawing.Graphics.FromImage(combined))
				{
					g.Clear(Color.Transparent);
					foreach (var tuple in bitmaps)
					{
						var bitmap = tuple.Item1;
						var offset = tuple.Item2;
						g.DrawImage(bitmap, offset.X, offset.Y, bitmap.Width, bitmap.Height);
					}
				}
				return combined;
			}
			finally
			{
				foreach (var bitmap in bitmaps)
					bitmap.Item1.Dispose();
			}
		}
	}
}
