namespace MonoHaven.Game.Messages
{
	public class WidgetDestroyMessage
	{
		public WidgetDestroyMessage(ushort id)
		{
			Id = id;
		}

		public ushort Id
		{
			get;
			private set;
		}
	}
}
