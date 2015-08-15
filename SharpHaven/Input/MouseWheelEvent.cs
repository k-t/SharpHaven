using System.Drawing;

namespace SharpHaven.Input
{
	public class MouseWheelEvent : MouseEvent
	{
		public MouseWheelEvent(int delta, Point position) : base(position)
		{
			Delta = delta;
		}

		public int Delta { get; }
	}
}
