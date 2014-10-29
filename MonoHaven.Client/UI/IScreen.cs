using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public interface IScreen
	{
		void Show();
		void Close();
		void Resize(int newWidth, int newHeight);
		void Draw(DrawingContext dc);
	}
}
