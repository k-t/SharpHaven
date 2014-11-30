using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.Input
{
	public class MouseMoveEvent : MouseEvent
	{
		private readonly Point delta;

		public MouseMoveEvent(Point delta, Point position) : base(position)
		{
			this.delta = delta;
		}

		public Point Delta
		{
			get { return delta; }
		}

		public int DeltaX
		{
			get { return delta.X; }
		}

		public int DeltaY
		{
			get { return delta.Y; }
		}
	}
}
