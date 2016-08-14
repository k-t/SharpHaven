using Haven;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI
{
	public interface IWidgetHost
	{
		Point2D MousePosition { get; }

		HotkeyManager Hotkeys { get; }

		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
		void DoDragDrop(Drag drag);
	}
}
