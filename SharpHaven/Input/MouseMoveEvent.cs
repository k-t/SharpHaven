using System.Drawing;

namespace SharpHaven.Input
{
	public class MouseMoveEvent : MouseEvent
	{
		public MouseMoveEvent(Point delta, Point position) : base(position)
		{
			Delta = delta;
		}

		public Point Delta { get; }

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
