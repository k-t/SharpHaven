namespace SharpHaven.Game.Events
{
	public class ResourceLoadEvent
	{
		public ushort Id { get; set; }
		public string Name { get; set; }
		public ushort Version { get; set; }
	}
}
