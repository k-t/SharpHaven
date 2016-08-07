using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class MouseMoveEvent : MouseEvent
	{
		public MouseMoveEvent(Coord2d delta, Coord2d position) : base(position)
		{
			Delta = delta;
		}

		public Coord2d Delta { get; }

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
