using System.Drawing;

namespace MonoHaven.Game.Messages
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
	}
}
