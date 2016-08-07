using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class MouseMoveEvent : MouseEvent
	{
		public MouseMoveEvent(Coord2D delta, Coord2D position) : base(position)
		{
			Delta = delta;
		}

		public Coord2D Delta { get; }

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
