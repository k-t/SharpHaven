using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI
{
	public interface IWidgetHost
	{
		Point MousePosition { get; }

		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
		void DoDragDrop(Drag drag);
	}
}
