namespace MonoHaven.UI
{
	public interface IWidgetHost
	{
		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
	}
}
