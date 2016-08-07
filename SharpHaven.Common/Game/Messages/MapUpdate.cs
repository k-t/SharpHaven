using SharpHaven.Graphics;

namespace SharpHaven.Game.Messages
{
	public class MapUpdate
	{
		public Coord2D Grid { get; set; }

		public string MinimapName { get; set; }

		public byte[] Tiles { get; set; }

		public int[] Overlays { get; set; }
	}
}