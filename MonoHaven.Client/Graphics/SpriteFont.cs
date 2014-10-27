using System;
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
		private readonly Dictionary<char, TextGlyph> glyphs;

		public SpriteFont(Face face, int pixelSize)
		{
			atlas = new TextureAtlas(512, 512);
			glyphs = new Dictionary<char, TextGlyph>();
			this.face = face;
			this.face.SetPixelSizes((uint)pixelSize, (uint)pixelSize);
		}

		public void Dispose()
		{
			atlas.Dispose();
		}

		public TextGlyph GetGlyph(char c)
		{
			TextGlyph glyph;
			if (!glyphs.TryGetValue(c, out glyph))
			{
				glyph = LoadGlyph(c);
				glyphs[c] = glyph;
			}
			return glyph;
		}

		private TextGlyph LoadGlyph(char c)
		{
			var index = face.GetCharIndex(c);
			face.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);
			
			var sz = new Size(face.Glyph.Metrics.Width >> 6, face.Glyph.Metrics.Height >> 6);
			if (sz == Size.Empty)
			{
				return new TextGlyph { Advance = face.Glyph.Advance.X / 64f };
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

				return new TextGlyph {
					Advance = face.Glyph.Advance.X / 64f,
					Offset = new Point(face.Glyph.BitmapLeft, -face.Glyph.BitmapTop),
					Image = atlas.Add(glyphPixels, PixelFormat.Rgba, sz.Width, sz.Height)
				};
			}
		}
	}
}
