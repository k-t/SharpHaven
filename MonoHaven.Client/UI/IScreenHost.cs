namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		int Width { get; }
		int Height { get; }

		void SetInputListener(IInputListener listener);
		void SetScreen(IScreen screen);
	}
}
