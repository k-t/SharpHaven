using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class MouseWheelEvent : MouseEvent
	{
		public MouseWheelEvent(int delta, Coord2D position) : base(position)
		{
			Delta = delta;
		}

		public int Delta { get; }
	}
}
