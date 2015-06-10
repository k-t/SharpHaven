using System.Drawing;

namespace SharpHaven.Game.Events
{
	public class MapUpdateEvent
	{
		public Point Grid { get; set; }
		public string MinimapName { get; set; }
		public byte[] Tiles { get; set; }
		public int[] Overlays { get; set; }
	}
}
