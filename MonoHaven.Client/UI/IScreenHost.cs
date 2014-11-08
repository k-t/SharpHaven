using System;
using OpenTK;

namespace MonoHaven.UI
{
	public interface IScreenHost
	{
		void SetCursor(MouseCursor cursor);
		void SetScreen(IScreen screen);
		void InvokeOnMainThread(Action action);
	}
}
