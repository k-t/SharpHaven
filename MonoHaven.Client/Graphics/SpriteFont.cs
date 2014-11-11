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

		public SpriteFont(Face face, uint pixelSize)
		{
			atlas = new TextureAtlas(512, 512);
			glyphs = new Dictionary<char, Glyph>();
			this.face = face;
			this.pixelSize = pixelSize;
		}

		public int Ascent
		{
			get
			{
				EnsureSize();
				return face.Size.Metrics.Ascender >> 6;
			}
		}

		public int Height
		{
			get
			{
				EnsureSize();
				return face.Size.Metrics.Height >> 6;
			}
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
				glyph = LoadGlyph(c);
				glyphs[c] = glyph;
			}
			return glyph;
		}

		private Glyph LoadGlyph(char c)
		{
			EnsureSize();

			var index = face.GetCharIndex(c);
			face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
			
			var sz = new Size(face.Glyph.Metrics.Width >> 6, face.Glyph.Metrics.Height >> 6);
			if (sz == Size.Empty)
			{
				return new Glyph { Advance = face.Glyph.Advance.X / 64f };
			}

			face.Glyph.RenderGlyph(RenderMode.Normal);

			using (var bitmap = face.Glyph.Bitmap)
			{
				var bufferData = bitmap.BufferData;
				var glyphPixels = new byte[4 * sz.Width * sz.Height];

				for (int j = 0; j < sz.Height; j++)
					for (int i = 0; i < sz.Width; i++)
					{
						int k = (i + bitmap.Width * j) * 4;
						glyphPixels[k] = 255;
						glyphPixels[k + 1] = 255;
						glyphPixels[k + 2] = 255;
						glyphPixels[k + 3] = bufferData[i + bitmap.Width * j];
					}

				return new Glyph {
					Advance = face.Glyph.Advance.X / 64f,
					Offset = new Point(face.Glyph.BitmapLeft, -face.Glyph.BitmapTop),
					Image = atlas.Add(glyphPixels, PixelFormat.Rgba, sz.Width, sz.Height)
				};
			}
		}

		private void EnsureSize()
		{
			face.SetPixelSizes(pixelSize, pixelSize);
		}
	}
}
