using System.Drawing;

namespace SharpHaven.Input
{
	public class DropEvent : MouseEvent
	{
		public DropEvent(Point mousePosition, object data) : base(mousePosition)
		{
			Data = data;
		}

		public object Data { get; }
	}
}
