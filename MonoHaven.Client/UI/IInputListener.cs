using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public interface IInputListener
	{
		void MouseButtonDown(MouseButtonEventArgs e);
		void MouseButtonUp(MouseButtonEventArgs e);
		void MouseMove(MouseMoveEventArgs e);
		void KeyDown(KeyboardKeyEventArgs e);
		void KeyUp(KeyboardKeyEventArgs e);
		void KeyPress(KeyPressEventArgs e);
	}
}
