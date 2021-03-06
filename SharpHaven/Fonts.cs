﻿using SharpFont;
using SharpHaven.Graphics;

namespace SharpHaven
{
	public static class Fonts
	{
		public static readonly SpriteFont Default;
		public static readonly SpriteFont LabelText;
		public static readonly SpriteFont Text;
		public static readonly SpriteFont Heading;
		public static readonly SpriteFont Tooltip;

		static Fonts()
		{
			Default = Create(FontFaces.Sans, 14);
			LabelText = Create(FontFaces.Sans, 11);
			Text = Create(FontFaces.Serif, 11);
			Heading = Create(FontFaces.Serif, 18);
			Tooltip = LabelText;
		}

		public static SpriteFont Create(Face fontFace, uint pixelSize)
		{
			return new SpriteFont(fontFace, pixelSize);
		}
	}
}
