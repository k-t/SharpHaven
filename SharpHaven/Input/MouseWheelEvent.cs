using Haven;

namespace SharpHaven.Input
{
	public class MouseWheelEvent : MouseEvent
	{
		public MouseWheelEvent(int delta, Point2D position) : base(position)
		{
			Delta = delta;
		}

		public int Delta { get; }
	}
}
