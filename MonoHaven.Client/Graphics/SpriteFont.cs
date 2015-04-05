﻿using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SharpFont;

namespace MonoHaven.Graphics
{
	public class SpriteFont : IDisposable
	{
		private readonly Face face;
		private readonly TextureAtlas atlas;
		private readonly Dictionary<char, Glyph> glyphs;
		private readonly uint pixelSize;
		private readonly int ascent;
		private readonly int height;

		public SpriteFont(Face face, uint pixelSize)
		{
			atlas = new TextureAtlas(512, 512);
			glyphs = new Dictionary<char, Glyph>();
			this.face = face;
			this.pixelSize = pixelSize;

			EnsureSize();
			using (var size = face.Size)
			{
				ascent = size.Metrics.Ascender.ToInt32();
				height = size.Metrics.Height.ToInt32();
			}
		}

		public int Ascent
		{
			get { return ascent; }
		}

		public int Height
		{
			get { return height; }
		}

		public void Dispose()
		{
			atlas.Dispose();
		}

		public Glyph GetGlyph(char c)
		{
			Glyph glyph;
			if (!glyphs.TryGetValue(c, out glyph))
			{
				glyph = GenerateGlyph(c);
				glyphs[c] = glyph;
			}
			return glyph;
		}

		private Glyph GenerateGlyph(char c)
		{
			LoadGlyph(c);

			var sz = new Size(face.Glyph.Metrics.Width.ToInt32(), face.Glyph.Metrics.Height.ToInt32());
			if (sz == Size.Empty)
				return new Glyph { Advance = face.Glyph.Advance.X.ToSingle() };

			face.Glyph.RenderGlyph(RenderMode.Normal);
			using (var bitmap = face.Glyph.Bitmap)
			{
				var bufferData = bitmap.BufferData;
				var image = new Pixmap(PixelFormat.Rgba, sz);
				for (int j = 0; j < sz.Height; j++)
					for (int i = 0; i < sz.Width; i++)
					{
						int k = (i + bitmap.Width * j) * 4;
						image.PixelData[k] = 255;
						image.PixelData[k + 1] = 255;
						image.PixelData[k + 2] = 255;
						image.PixelData[k + 3] = bufferData[i + bitmap.Width * j];
					}
				return new Glyph {
					Advance = face.Glyph.Advance.X.ToSingle(),
					Offset = new Point(face.Glyph.BitmapLeft, -face.Glyph.BitmapTop),
					Image = atlas.Add(image)
				};
			}
		}

		private void LoadGlyph(char c)
		{
			EnsureSize();
			var index = face.GetCharIndex(c);
			face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
		}

		private void EnsureSize()
		{
			face.SetPixelSizes(pixelSize, pixelSize);
		}
	}
}
