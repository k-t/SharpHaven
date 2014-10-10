namespace MonoHaven.Resources
{
	public class ImageLayer : ILayer
	{
		public short Id { get; set; }
		public short Z { get; set; }
		public short SubZ { get; set; }
		public byte[] Data { get; set; }
	}
}

