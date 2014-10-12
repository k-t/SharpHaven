namespace MonoHaven.Resources.Layers
{
	public class TilesetLayer : ILayer
	{
		public bool HasTransitions { get; set; }
		public FlavorObjectInfo[] FlavorObjects { get; set; }
		public ushort FlavorDensity { get; set; }
	}

	public class FlavorObjectInfo
	{
		public string ResName { get; set; }
		public ushort ResVersion { get; set; }
		public ushort Weight { get; set; }
	}
}
