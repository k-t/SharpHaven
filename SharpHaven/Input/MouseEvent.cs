using Haven;

namespace SharpHaven.Input
{
	public abstract class MouseEvent : InputEvent
	{
		protected MouseEvent(Point2D position)
		{
			this.Position = position;
		}

		public Point2D Position { get; }

		public int X
		{
			get { return Position.X; }
		}

		public int Y
		{
			get { return Position.Y; }
		}
	}
}
