namespace Haven
{
	public struct Color
	{
		public static readonly Color Transparent = FromArgb(0, 0, 0, 0);
		public static readonly Color White = FromArgb(255, 255, 255);
		public static readonly Color Black = FromArgb(0, 0, 0);
		public static readonly Color Red = FromArgb(255, 0, 0);
		public static readonly Color Green = FromArgb(0, 255, 0);
		public static readonly Color Blue = FromArgb(0, 0, 255);
		public static readonly Color Yellow = FromArgb(255, 255, 0);

		public byte A { get; set; }
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }

		public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
		{
			return new Color { A = alpha, B = blue, R = red, G = green };
		}

		public static Color FromArgb(byte red, byte green, byte blue)
		{
			return FromArgb(255, red, green, blue);
		}

		public static Color FromArgb(byte alpha, Color color)
		{
			return new Color { A = alpha, B = color.B, R = color.R, G = color.G };
		}
	}
}
