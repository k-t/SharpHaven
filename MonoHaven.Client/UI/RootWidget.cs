using System;
using MonoHaven.Input;

namespace MonoHaven.UI
{
	public class RootWidget : Widget
	{
		public RootWidget(IWidgetHost host) : base(host)
		{
		}

		public event Action<KeyEvent> UnhandledKeyDown;
		public event Action<KeyEvent> UnhandledKeyUp;

		protected override void OnKeyDown(KeyEvent e)
		{
			UnhandledKeyDown.Raise(e);
		}

		protected override void OnKeyUp(KeyEvent e)
		{
			UnhandledKeyUp.Raise(e);
		}
	}
}
