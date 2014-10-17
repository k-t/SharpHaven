using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public interface IScreen
	{
		void Show();
		void Close();

		void Resize(int newWidth, int newHeight);
		void Draw(DrawingContext drawingContext);

		void HandleMouseButtonDown(MouseButtonEventArgs e);
		void HandleMouseButtonUp(MouseButtonEventArgs e);
		void HandleMouseMove(MouseMoveEventArgs e);
		void HandleKeyDown(KeyboardKeyEventArgs e);
		void HandleKeyUp(KeyboardKeyEventArgs e);
	}
}
