using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI
{
	public interface IWidgetHost
	{
		Coord2D MousePosition { get; }

		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
		void DoDragDrop(Drag drag);
	}
}
