using SharpHaven.Network;

namespace SharpHaven.Game.Events
{
	public class WidgetMessage
	{
		public WidgetMessage(ushort id, string name, object[] args)
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

		public static WidgetMessage ReadFrom(MessageReader reader)
		{
			return new WidgetMessage(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				args: reader.ReadList()
			);
		}
	}
}
