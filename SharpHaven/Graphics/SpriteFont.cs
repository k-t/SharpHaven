using System;
using System.Collections.Generic;
using System.Drawing;
using Haven;
using OpenTK.Graphics.OpenGL;
using SharpFont;

namespace SharpHaven.Graphics
{
	public class SpriteFont : IDisposable
	{
		private readonly Face face;
		private readonly TextureAtlas atlas;
		private readonly Dictionary<char, Glyph> glyphs;
		private readonly Dictionary<char, Glyph> outlines;
		private readonly uint pixelSize;
		private readonly int ascent;
		private readonly int height;

		public SpriteFont(Face face, uint pixelSize)
		{
			this.atlas = new TextureAtlas(512, 512);
			this.glyphs = new Dictionary<char, Glyph>();
			this.outlines = new Dictionary<char, Glyph>();
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

		public Glyph GetGlyphOutline(char c)
		{
			Glyph outline;
			if (!outlines.TryGetValue(c, out outline))
			{
				outline = GenerateOutline(c);
				outlines[c] = outline;
			}
			return outline;
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
				var image = ToPixmap(bitmap);
				return new Glyph {
					Advance = face.Glyph.Advance.X.ToSingle(),
					Offset = new Point2D(face.Glyph.BitmapLeft, -face.Glyph.BitmapTop),
					Image = atlas.Add(image)
				};
			}
		}

		private Glyph GenerateOutline(char c)
		{
			LoadGlyph(c);

			var sz = new Size(face.Glyph.Metrics.Width.ToInt32(), face.Glyph.Metrics.Height.ToInt32());
			if (sz == Size.Empty)
				return new Glyph { Advance = face.Glyph.Advance.X.ToSingle() };

			using (var glyph = face.Glyph.GetGlyph())
			using (var stroker = new Stroker(face.Glyph.Library))
			{
				stroker.Set(64 * 1, StrokerLineCap.Round, StrokerLineJoin.Round, 0);
				using (var strokedGlyph = glyph.StrokeBorder(stroker, false, false))
				{
					strokedGlyph.ToBitmap(RenderMode.Normal, new FTVector26Dot6(), false);
					var bbox = strokedGlyph.GetCBox(GlyphBBoxMode.Pixels);
					var image = ToPixmap(strokedGlyph.ToBitmapGlyph().Bitmap);
					return new Glyph
					{
						Advance = strokedGlyph.Advance.X.ToSingle(),
						Offset = new Point2D(bbox.Left, -bbox.Top),
						Image = atlas.Add(image)
					};
				}
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

		private static Pixmap ToPixmap(FTBitmap bitmap)
		{
			var bufferData = bitmap.BufferData;
			var size = new Size(bitmap.Width, bitmap.Rows);
			var pixmap = new Pixmap(PixelFormat.Rgba, size);
			for (int j = 0; j < size.Height; j++)
				for (int i = 0; i < size.Width; i++)
				{
					int k = (i + bitmap.Width * j) * 4;
					pixmap.PixelData[k] = 255;
					pixmap.PixelData[k + 1] = 255;
					pixmap.PixelData[k + 2] = 255;
					pixmap.PixelData[k + 3] = bufferData[i + bitmap.Width * j];
				}
			return pixmap;
		}
	}
}
