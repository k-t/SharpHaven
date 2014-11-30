using System.Drawing;

namespace MonoHaven.Input
{
	public class MouseWheelEvent : MouseEvent
	{
		private readonly int delta;

		public MouseWheelEvent(int delta, Point position) : base(position)
		{
			this.delta = delta;
		}

		public int Delta
		{
			get { return delta; }
		}
	}
}
