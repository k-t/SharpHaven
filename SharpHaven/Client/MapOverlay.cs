using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class MapOverlay
	{
		public MapOverlay(int index)
		{
			Index = index;
		}

		public int Index { get; }

		public Rect Bounds { get; set; }
	}
}
