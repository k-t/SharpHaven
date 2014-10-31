using System.Drawing;

namespace MonoHaven.Graphics
{
	public class Glyph
	{
		public float Advance
		{
			get;
			set;
		}

		public Drawable Image
		{
			get;
			set;
		}

		public Point Offset
		{
			get;
			set;
		}
	}
}
