using System.Drawing;

namespace SharpHaven.Messages
{
	public class UpdateMapMessage
	{
		public Point Grid { get; set; }
		public string MinimapName { get; set; }
		public byte[] Tiles { get; set; }
		public int[] Overlays { get; set; }
	}
}
