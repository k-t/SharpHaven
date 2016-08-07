using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public abstract class MouseEvent : InputEvent
	{
		protected MouseEvent(Coord2d position)
		{
			this.Position = position;
		}

		public Coord2d Position { get; }

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
