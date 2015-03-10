namespace MonoHaven.Network.Messages
{
	public class ActionDelta
	{
		public bool RemoveFlag { get; set; }
		public string Name { get; set; }
		public ushort Version { get; set; }
	}
}
