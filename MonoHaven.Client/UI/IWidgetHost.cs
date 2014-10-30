namespace MonoHaven.UI
{
	public interface IWidgetHost
	{
		void RequestKeyboardFocus(Widget widget);
		void RequestMouseFocus(Widget widget);
		void ReleaseMouse();
	}
}
