using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class DropEvent : MouseEvent
	{
		public DropEvent(Coord2D mousePosition, object data) : base(mousePosition)
		{
			Data = data;
		}

		public object Data { get; }
	}
}
