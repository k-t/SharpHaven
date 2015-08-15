using System.Drawing;

namespace SharpHaven.Client
{
	public class MapOverlay
	{
		public MapOverlay(int index)
		{
			Index = index;
		}

		public int Index { get; }

		public Rectangle Bounds { get; set; }
	}
}
