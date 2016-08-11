using Haven;
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
			return new MouseButtonEvent(e.Button, new Point2D(e.Position.X, e.Position.Y));
		}

		public static MouseMoveEvent Map(MouseMoveEventArgs e)
		{
			return new MouseMoveEvent(new Point2D(e.XDelta, e.YDelta), new Point2D(e.Position.X, e.Position.Y));
		}

		public static MouseWheelEvent Map(MouseWheelEventArgs e)
		{
			return new MouseWheelEvent(e.Delta, new Point2D(e.Position.X, e.Position.Y));
		}
	}
}
