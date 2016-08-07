using OpenTK;
using OpenTK.Input;
using SharpHaven.Graphics;

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
			return new MouseButtonEvent(e.Button, new Coord2d(e.Position.X, e.Position.Y));
		}

		public static MouseMoveEvent Map(MouseMoveEventArgs e)
		{
			return new MouseMoveEvent(new Coord2d(e.XDelta, e.YDelta), new Coord2d(e.Position.X, e.Position.Y));
		}

		public static MouseWheelEvent Map(MouseWheelEventArgs e)
		{
			return new MouseWheelEvent(e.Delta, new Coord2d(e.Position.X, e.Position.Y));
		}
	}
}
