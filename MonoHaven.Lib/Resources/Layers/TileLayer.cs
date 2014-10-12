namespace MonoHaven.Resources.Layers
{
	public class TileLayer : ILayer
	{
		public int Id { get; set; }
		public int Weight { get; set; }
		public char Type { get; set; }
		public byte[] ImageData { get; set; }
	}
}
