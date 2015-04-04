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
			Point position,
			object[] args)
		{
			Id = id;
			ParentId = parentId;
			Type = type;
			Position = position;
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

		public Point Position
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
				position: reader.ReadCoord(),
				parentId: reader.ReadUint16(),
				args: reader.ReadList()
			);
		}
	}
}
