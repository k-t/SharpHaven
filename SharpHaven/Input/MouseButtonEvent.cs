using Haven;
using OpenTK.Input;

namespace SharpHaven.Input
{
	public class MouseButtonEvent : MouseEvent
	{
		public MouseButtonEvent(MouseButton button, Point2D position)
			: base(position)
		{
			Button = button;
		}

		public MouseButton Button { get; }
	}
}
