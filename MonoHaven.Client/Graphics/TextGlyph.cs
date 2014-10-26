using System.Drawing;

namespace MonoHaven.Graphics
{
	public class TextGlyph
	{
		public float Advance { get; set; }
		public Point Offset { get; set; }
		public Drawable Image { get; set; }
	}
}
