﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public class SpriteSheet : IEnumerable<Picture>, IDisposable
	{
		private Texture tex;
		private readonly List<Picture> parts;

		public SpriteSheet(IEnumerable<ImageData> images, Point center)
		{
			parts = new List<Picture>();
			Pack(images.ToArray(), center);
		}

		public void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}

		public IEnumerator<Picture> GetEnumerator()
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
					parts.Add(new Picture(img.Id, slice, off, regions[i].Size,
						img.Z, img.SubZ, hitmasks[i]));
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

			private int packWidth;
			private int packHeight;

			public int PackWidth
			{
				get { return packWidth; }
			}

			public int PackHeight
			{
				get { return packHeight; }
			}

			public Rectangle Add(Size sz)
			{
				var rect = new Rectangle(packWidth, 0, sz.Width, sz.Height);
				packWidth += sz.Width + Padding;
				packHeight = Math.Max(packHeight, sz.Height);
				return rect;
			}
		}
	}
}
