using System.Drawing;

namespace MonoHaven.Resources
{
	public class ImageData
	{
		public short Id { get; set; }
		public short Z { get; set; }
		public short SubZ { get; set; }
		public byte[] Data { get; set; }
		public Point Offset { get; set; }
		public bool IsLayered { get; set; }
	}
}

