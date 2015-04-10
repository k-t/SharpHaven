using System.Drawing;

namespace SharpHaven.Input
{
	public abstract class MouseEvent : InputEvent
	{
		private readonly Point position;

		protected MouseEvent(Point position)
		{
			this.position = position;
		}

		public Point Position
		{
			get { return position; }
		}

		public int X
		{
			get { return position.X; }
		}

		public int Y
		{
			get { return position.Y; }
		}
	}
}
