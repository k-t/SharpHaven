using MonoHaven.Network;

namespace MonoHaven.Messages
{
	public class WidgetUpdateMessage
	{
		public WidgetUpdateMessage(ushort id, string name, object[] args)
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

		public static WidgetUpdateMessage ReadFrom(MessageReader reader)
		{
			return new WidgetUpdateMessage(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				args: reader.ReadList()
			);
		}
	}
}
