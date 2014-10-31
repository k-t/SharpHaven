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

		public void Show()
		{
		}

		public void Close()
		{
		}

		public void Resize(int newWidth, int newHeight)
		{
		}

		public void Draw(DrawingContext dc)
		{
		}
		
		public void MouseButtonDown(MouseButtonEventArgs e)
		{
		}
		
		public void MouseButtonUp(MouseButtonEventArgs e)
		{
		}
		
		public void MouseMove(MouseMoveEventArgs e)
		{
		}
		
		public void KeyDown(KeyEventArgs e)
		{
		}
		
		public void KeyUp(KeyEventArgs e)
		{
		}
		
		public void KeyPress(KeyPressEventArgs e)
		{
		}
	}
}
