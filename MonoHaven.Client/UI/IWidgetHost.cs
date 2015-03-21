using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI
{
	public interface IWidgetHost
	{
		Point MousePosition { get; }

		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
	}
}
