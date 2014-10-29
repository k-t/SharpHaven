using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	/// <summary>
	/// Screen that does nothing.
	/// </summary>
	public class EmptyScreen : IScreen
	{
		public static readonly EmptyScreen Instance = new EmptyScreen();

		public void Show() {}
		public void Close() {}
		public void Resize(int newWidth, int newHeight) {}
		public void Draw(DrawingContext dc) {}
		public void HandleMouseButtonDown(MouseButtonEventArgs e) {}
		public void HandleMouseButtonUp(MouseButtonEventArgs e) {}
		public void HandleMouseMove(MouseMoveEventArgs e) {}
		public void HandleKeyDown(KeyboardKeyEventArgs e) {}
		public void HandleKeyUp(KeyboardKeyEventArgs e) {}
	}
}
