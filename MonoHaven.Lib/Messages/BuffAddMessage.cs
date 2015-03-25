namespace MonoHaven.Messages
{
	public class BuffAddMessage
	{
		public int Id { get; set; }
		public int ResId { get; set; }
		public string Tooltip { get; set; }
		public int AMeter { get; set; }
		public int NMeter { get; set; }
		public int CMeter { get; set; }
		public int CTicks { get; set; }
		public long Time { get; set; }
		public bool Major { get; set; }
	}
}
