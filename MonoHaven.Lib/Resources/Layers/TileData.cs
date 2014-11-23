namespace MonoHaven.Resources.Layers
{
	public class TileData : IDataLayer
	{
		public int Id { get; set; }
		public int Weight { get; set; }
		public char Type { get; set; }
		public byte[] ImageData { get; set; }
	}
}
