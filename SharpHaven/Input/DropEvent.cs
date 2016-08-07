using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class DropEvent : MouseEvent
	{
		public DropEvent(Coord2d mousePosition, object data) : base(mousePosition)
		{
			Data = data;
		}

		public object Data { get; }
	}
}
