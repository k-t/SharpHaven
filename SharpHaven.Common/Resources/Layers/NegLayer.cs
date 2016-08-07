using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class NegLayer
	{
		public Coord2d Center { get; set; }
		public Rect Hitbox { get; set; }
		public Coord2d Sz { get; set; }
		public Coord2d[][] Ep { get; set; } /* points of E? */
	}
}
