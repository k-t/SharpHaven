﻿#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
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

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void Pack(ImageData[] images, Point center)
		{
			var bitmaps = new Bitmap[images.Length];
			var regions = new Rectangle[images.Length];
			try
			{
				var packer = new NaiveHorizontalPacker();
				// calculate pack
				for (int i = 0; i < images.Length; i++)
					using (var ms = new MemoryStream(images[i].Data))
					{
						bitmaps[i] = MakeTransparent(new Bitmap(ms));
						regions[i] = packer.Add(bitmaps[i].Size);
					}
				tex = new Texture(packer.PackWidth, packer.PackHeight);
				// add sprite parts to the texture
				for (int i = 0; i < images.Length; i++)
				{
					var slice = new TextureSlice(tex, regions[i]);
					slice.Update(bitmaps[i]);
					var img = images[i];
					var off = Point.Subtract(img.DrawOffset, (Size)center);
					parts.Add(new SpritePart(img.Id, slice, off, regions[i].Size, img.Z, img.SubZ));
				}
			}
			finally
			{
				foreach (var bitmap in bitmaps.Where(x => x != null))
					bitmap.Dispose();
			}
		}

		private static Bitmap MakeTransparent(Bitmap bitmap)
		{
			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
					if ((bitmap.GetPixel(i, j).ToArgb() & 0x00ffffff) == 0x00ff0080)
						bitmap.SetPixel(i, j, Color.FromArgb(128, 0, 0, 0));
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
