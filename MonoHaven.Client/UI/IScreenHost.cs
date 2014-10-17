namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		int Width { get; }
		int Height { get; }

		IScreen CurrentScreen { get; set; }
	}
}
