namespace SharpHaven.Game.Events
{
	public class PlaySoundEvent
	{
		public ushort ResourceId
		{
			get;
			set;
		}

		public double Volume
		{
			get;
			set;
		}

		public double Speed
		{
			get;
			set;
		}
	}
}
