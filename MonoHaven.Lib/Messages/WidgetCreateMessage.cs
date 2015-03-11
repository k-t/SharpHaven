using System.Drawing;
using MonoHaven.Network;

namespace MonoHaven.Messages
{
	public class WidgetCreateMessage
	{
		public WidgetCreateMessage(
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

		public static WidgetCreateMessage ReadFrom(MessageReader reader)
		{
			return new WidgetCreateMessage(
				id: reader.ReadUint16(),
				type: reader.ReadString(),
				location: reader.ReadCoord(),
				parentId: reader.ReadUint16(),
				args: reader.ReadList()
			);
		}
	}
}
