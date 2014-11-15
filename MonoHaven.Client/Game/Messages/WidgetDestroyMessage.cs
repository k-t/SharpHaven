using MonoHaven.Network;

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

		public static WidgetDestroyMessage ReadFrom(MessageReader reader)
		{
			return new WidgetDestroyMessage(id: reader.ReadUint16());
		}
	}
}
