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

		protected override bool OnKeyDown(KeyEvent e)
		{
			UnhandledKeyDown.Raise(e);
			return true;
		}

		protected override bool OnKeyUp(KeyEvent e)
		{
			UnhandledKeyUp.Raise(e);
			return true;
		}
	}
}
