namespace SharpHaven.Game.Events
{
	public class TilesetLoadEvent
	{
		public byte Id { get; set; }
		public string Name { get; set; }
		public ushort Version { get; set; }
	}
}
