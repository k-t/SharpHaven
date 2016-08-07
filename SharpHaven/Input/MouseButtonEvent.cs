using OpenTK.Input;
using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class MouseButtonEvent : MouseEvent
	{
		public MouseButtonEvent(MouseButton button, Coord2d position)
			: base(position)
		{
			Button = button;
		}

		public MouseButton Button { get; }
	}
}
