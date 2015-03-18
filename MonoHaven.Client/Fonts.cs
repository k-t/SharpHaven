using MonoHaven.Graphics;
using SharpFont;

namespace MonoHaven
{
	public static class Fonts
	{
		public static readonly SpriteFont Default;
		public static readonly SpriteFont LabelText;
		public static readonly SpriteFont Text;
		public static readonly SpriteFont Heading;

		static Fonts()
		{
			Default = Create(FontFaces.Sans, 14);
			LabelText = Create(FontFaces.Sans, 11);
			Text = Create(FontFaces.Serif, 12);
			Heading = Create(FontFaces.Serif, 18);
		}

		public static SpriteFont Create(Face fontFace, uint pixelSize)
		{
			return new SpriteFont(fontFace, pixelSize);
		}
	}
}
