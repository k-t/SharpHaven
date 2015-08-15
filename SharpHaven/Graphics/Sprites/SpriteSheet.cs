using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SharpHaven.Resources;

namespace SharpHaven.Graphics.Sprites
{
	public class SpriteSheet : IEnumerable<SpritePart>, IDisposable
	{
		private Texture tex;
		private readonly List<SpritePart> parts;

		public SpriteSheet(IEnumerable<ImageData> images, Point center)
		{
			parts = new List<SpritePart>();
			Pack(images.ToArray(), center);
		}

		public void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}

		public IEnumerator<SpritePart> GetEnumerator()
		{
			return parts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void Pack(ImageData[] images, Point center)
		{
			var bitmaps = new Bitmap[images.Length];
			var regions = new Rectangle[images.Length];
			var hitmasks = new BitArray[images.Length];
			try
			{
				var packer = new NaiveHorizontalPacker();
				// calculate pack
				for (int i = 0; i < images.Length; i++)
					using (var ms = new MemoryStream(images[i].Data))
					{
						BitArray hitmask;
						bitmaps[i] = ProcessImage(new Bitmap(ms), out hitmask);
						regions[i] = packer.Add(bitmaps[i].Size);
						hitmasks[i] = hitmask;
					}
				tex = new Texture(packer.PackWidth, packer.PackHeight);
				// add sprite parts to the texture
				for (int i = 0; i < images.Length; i++)
				{
					var slice = new TextureSlice(tex, regions[i]);
					slice.Update(bitmaps[i]);
					var img = images[i];
					var off = Point.Subtract(img.Offset, (Size)center);
					var pic = new Picture(slice, hitmasks[i]);
					parts.Add(new SpritePart(img.Id, pic, off, img.Z, img.SubZ));
				}
			}
			finally
			{
				foreach (var bitmap in bitmaps.Where(x => x != null))
					bitmap.Dispose();
			}
		}

		private static Bitmap ProcessImage(Bitmap bitmap, out BitArray hitmask)
		{
			hitmask = new BitArray(bitmap.Width * bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
				{
					var pixel = bitmap.GetPixel(i, j);
					if ((pixel.ToArgb() & 0x00ffffff) == 0x00ff0080)
						bitmap.SetPixel(i, j, Color.FromArgb(128, 0, 0, 0));
					hitmask[i * bitmap.Height + j] = pixel.A > 128;
				}
			return bitmap;
		}

		private class NaiveHorizontalPacker
		{
			private const int Padding = 1;

			public int PackWidth { get; private set; }

			public int PackHeight { get; private set; }

			public Rectangle Add(Size sz)
			{
				var rect = new Rectangle(PackWidth, 0, sz.Width, sz.Height);
				PackWidth += sz.Width + Padding;
				PackHeight = Math.Max(PackHeight, sz.Height);
				return rect;
			}
		}
	}
}
