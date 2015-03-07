using System.Drawing;

namespace MonoHaven.Resources
{
	public class NegData
	{
		public Point Center { get; set; }
		public Point Bc { get; set; }     /* box coord? */
		public Point Bs { get; set; }     /* box size? */
		public Point Sz { get; set; }
		public Point[][] Ep { get; set; } /* points of E? */
	}
}
