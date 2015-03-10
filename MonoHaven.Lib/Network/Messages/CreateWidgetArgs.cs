using System.Drawing;

namespace MonoHaven.Network.Messages
{
	public class CreateWidgetArgs
	{
		public CreateWidgetArgs(
			ushort id,
			ushort parentId,
			string type,
			Point location,
			object[] args)
		{
			Id = id;
			ParentId = parentId;
			Type = type;
			Location = location;
			Args = args;
		}

		public ushort Id
		{
			get;
			private set;
		}

		public ushort ParentId
		{
			get;
			private set;
		}

		public string Type
		{
			get;
			private set;
		}

		public Point Location
		{
			get;
			private set;
		}

		public object[] Args
		{
			get;
			private set;
		}

		public static CreateWidgetArgs ReadFrom(MessageReader reader)
		{
			return new CreateWidgetArgs(
				id: reader.ReadUint16(),
				type: reader.ReadString(),
				location: reader.ReadCoord(),
				parentId: reader.ReadUint16(),
				args: reader.ReadList()
			);
		}
	}
}
