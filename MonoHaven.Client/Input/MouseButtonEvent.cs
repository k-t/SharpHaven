using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.Input
{
	public class MouseButtonEvent : MouseEvent
	{
		private readonly MouseButton button;

		public MouseButtonEvent(MouseButton button, Point position)
			: base(position)
		{
			this.button = button;
		}

		public MouseButton Button
		{
			get { return button; }
		}
	}
}
