namespace MonoHaven.Network.Messages
{
	public class ResourceBinding
	{
		public ResourceBinding(ushort id, string name, ushort version)
		{
			Id = id;
			Name = name;
			Version = version;
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

		public ushort Version
		{
			get;
			private set;
		}

		public static ResourceBinding ReadFrom(MessageReader reader)
		{
			return new ResourceBinding(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				version: reader.ReadUint16()
			);
		}
	}
}
