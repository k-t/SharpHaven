namespace MonoHaven.Resources
{
	public class TilesetData
	{
		public bool HasTransitions { get; set; }
		public FlavorObjectData[] FlavorObjects { get; set; }
		public ushort FlavorDensity { get; set; }
	}

	public struct FlavorObjectData
	{
		public string ResName { get; set; }
		public ushort ResVersion { get; set; }
		public byte Weight { get; set; }
	}
}
