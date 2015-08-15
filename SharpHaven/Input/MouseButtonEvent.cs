using System.Drawing;
using OpenTK.Input;

namespace SharpHaven.Input
{
	public class MouseButtonEvent : MouseEvent
	{
		public MouseButtonEvent(MouseButton button, Point position)
			: base(position)
		{
			Button = button;
		}

		public MouseButton Button { get; }
	}
}
