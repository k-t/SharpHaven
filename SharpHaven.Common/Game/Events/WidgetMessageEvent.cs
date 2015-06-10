using SharpHaven.Network;

namespace SharpHaven.Game.Events
{
	public class WidgetMessageEvent
	{
		public WidgetMessageEvent(ushort id, string name, object[] args)
		{
			Id = id;
			Name = name;
			Args = args;
		}

		public ushort Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public object[] Args
		{
			get;
			private set;
		}

		public static WidgetMessageEvent ReadFrom(MessageReader reader)
		{
			return new WidgetMessageEvent(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				args: reader.ReadList()
			);
		}
	}
}
