using Haven;

namespace SharpHaven.Input
{
	public class DropEvent : MouseEvent
	{
		public DropEvent(Point2D mousePosition, object data) : base(mousePosition)
		{
			Data = data;
		}

		public object Data { get; }
	}
}
