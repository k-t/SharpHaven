namespace MonoHaven.Messages
{
	public class ActionMessage
	{
		public bool RemoveFlag { get; set; }
		public string Name { get; set; }
		public ushort Version { get; set; }
	}
}
