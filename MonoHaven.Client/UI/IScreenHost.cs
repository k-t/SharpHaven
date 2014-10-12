using System;

namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		int Width { get; }
		int Height { get; }

		event EventHandler Resized;
	}
}
