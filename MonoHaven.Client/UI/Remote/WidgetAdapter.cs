namespace MonoHaven.UI.Remote
{
	public abstract class WidgetAdapter
	{
		public abstract Widget Create(Widget parent, object[] args);

		public virtual void HandleMessage(Widget widget, string message, object[] args)
		{
			// TODO: handle common widget commands (focus, tab, etc).
		}
	}
}
