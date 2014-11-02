using OpenTK;

namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		int Width { get; }
		int Height { get; }

		void SetCursor(MouseCursor cursor);
		void SetScreen(IScreen screen);
	}
}
