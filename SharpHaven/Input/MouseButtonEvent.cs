using OpenTK.Input;
using SharpHaven.Graphics;

namespace SharpHaven.Input
{
	public class MouseButtonEvent : MouseEvent
	{
		public MouseButtonEvent(MouseButton button, Coord2D position)
			: base(position)
		{
			Button = button;
		}

		public MouseButton Button { get; }
	}
}
