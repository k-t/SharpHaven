using System.Drawing;

namespace SharpHaven.Input
{
	public abstract class MouseEvent : InputEvent
	{
		protected MouseEvent(Point position)
		{
			this.Position = position;
		}

		public Point Position { get; }

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
