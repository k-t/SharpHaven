using System.Drawing;
using SharpHaven.Net;

namespace SharpHaven.Game.Events
{
	public class WidgetCreateEvent
	{
		public WidgetCreateEvent(
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

		public static WidgetCreateEvent ReadFrom(MessageReader reader)
		{
			var id = reader.ReadUint16();
			var type = reader.ReadString();
			var position = reader.ReadCoord();
			var parentId = reader.ReadUint16();
			var args = reader.ReadList();

			return new WidgetCreateEvent(
				id: id,
				type: type,
				position: position,
				parentId: parentId,
				args: args
			);
		}
	}
}
