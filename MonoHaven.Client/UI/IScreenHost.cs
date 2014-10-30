namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		int Width { get; }
		int Height { get; }

		void SetScreen(IScreen screen);
	}
}
