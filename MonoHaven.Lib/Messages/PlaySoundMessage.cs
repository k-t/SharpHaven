namespace SharpHaven.Messages
{
	public class PlaySoundMessage
	{
		public ushort ResourceId { get; set; }
		public double Volume { get; set; }
		public double Speed { get; set; }
	}
}
