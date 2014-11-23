namespace MonoHaven.Resources.Layers
{
	public class TilesetData : IDataLayer
	{
		public bool HasTransitions { get; set; }
		public FlavorObjectData[] FlavorObjects { get; set; }
		public ushort FlavorDensity { get; set; }
	}

	public class FlavorObjectData
	{
		public string ResName { get; set; }
		public ushort ResVersion { get; set; }
		public ushort Weight { get; set; }
	}
}
