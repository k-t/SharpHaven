namespace MonoHaven.Network.Messages
{
	public class UpdateWidgetArgs
	{
		public UpdateWidgetArgs(ushort id, string name, object[] args)
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

		public static UpdateWidgetArgs ReadFrom(MessageReader reader)
		{
			return new UpdateWidgetArgs(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				args: reader.ReadList()
			);
		}
	}
}
