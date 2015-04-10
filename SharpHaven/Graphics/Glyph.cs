using System.Drawing;

namespace SharpHaven.Graphics
{
	public class Glyph
	{
		public float Advance
		{
			get;
			set;
		}

		public TextureSlice Image
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
