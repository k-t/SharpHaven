﻿namespace Haven.Legacy.Messages
{
	public class PlaySound
	{
		public ushort ResourceId { get; set; }

		public double Volume { get; set; }

		public double Speed { get; set; }
	}
}