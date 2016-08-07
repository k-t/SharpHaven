using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class NegLayer
	{
		public Coord2D Center { get; set; }
		public Rect Hitbox { get; set; }
		public Coord2D Sz { get; set; }
		public Coord2D[][] Ep { get; set; } /* points of E? */
	}
}
