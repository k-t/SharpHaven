using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace SharpHaven.Input
{
	public static class InputConverter
	{
		public static KeyEvent Map(KeyboardKeyEventArgs e)
		{
			return new KeyEvent(e.Key);
		}

		public static KeyPressEvent Map(KeyPressEventArgs e)
		{
			return new KeyPressEvent(e.KeyChar);
		}

		public static MouseButtonEvent Map(MouseButtonEventArgs e)
		{
			return new MouseButtonEvent(e.Button, e.Position);
		}

		public static MouseMoveEvent Map(MouseMoveEventArgs e)
		{
			return new MouseMoveEvent(new Point(e.XDelta, e.YDelta), e.Position);
		}

		public static MouseWheelEvent Map(MouseWheelEventArgs e)
		{
			return new MouseWheelEvent(e.Delta, e.Position);
		}
	}
}
