using System.Drawing;

namespace MonoHaven.Resources
{
	public class NegData
	{
		public Point Center { get; set; }
		public Rectangle Hitbox { get; set; }
		public Point Sz { get; set; }
		public Point[][] Ep { get; set; } /* points of E? */
	}
}
