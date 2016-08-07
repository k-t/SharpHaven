using SharpHaven.Graphics;

namespace SharpHaven.Game.Messages
{
	public class MapUpdateGrid
	{
		public Coord2D Coord { get; set; }

		public string MinimapName { get; set; }

		public byte[] Tiles { get; set; }

		public int[] Overlays { get; set; }
	}
}