using System.Drawing;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI
{
	public interface IScreen
	{
		void Show();
		void Close();
		void Resize(int newWidth, int newHeight);
		void Draw(DrawingContext dc);
		void Update(int dt);

		void MouseButtonDown(MouseButtonEvent e);
		void MouseButtonUp(MouseButtonEvent e);
		void MouseMove(MouseMoveEvent e);
		void MouseWheel(MouseWheelEvent e);
		void KeyDown(KeyEvent e);
		void KeyUp(KeyEvent e);
		void KeyPress(KeyPressEvent e);
	}

	public static class ScreenExt
	{
		public static void Resize(this IScreen screen, Size newSize)
		{
			screen.Resize(newSize.Width, newSize.Height);
		}
	}
}
