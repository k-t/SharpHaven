namespace MonoHaven.Resources.Layers
{
	public class AnimData : IDataLayer
	{
		public short Id { get; set; }
		public ushort Duration { get; set; }
		public short[] Frames { get; set; }
	}
}
