using System.Drawing;
using SharpHaven.Net;

namespace SharpHaven.Game.Events
{
	public class WidgetCreateEvent
	{
		public ushort Id
		{
			get;
			set;
		}

		public ushort ParentId
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public Point Position
		{
			get;
			set;
		}
		
		public object[] Args
		{
			get;
			set;
		}
	}
}
