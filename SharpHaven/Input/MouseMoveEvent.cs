using Haven;

namespace SharpHaven.Input
{
	public class MouseMoveEvent : MouseEvent
	{
		public MouseMoveEvent(Point2D delta, Point2D position) : base(position)
		{
			Delta = delta;
		}

		public Point2D Delta { get; }

		public int DeltaX
		{
			get { return Delta.X; }
		}

		public int DeltaY
		{
			get { return Delta.Y; }
		}
	}
}
