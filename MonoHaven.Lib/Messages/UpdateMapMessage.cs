using System.Drawing;

namespace MonoHaven.Messages
{
	public class UpdateMapMessage
	{
		public Point Grid { get; set; }
		public string MinimapName { get; set; }
		public byte[] Tiles { get; set; }
		public int[] Overlays { get; set; }
	}
}
