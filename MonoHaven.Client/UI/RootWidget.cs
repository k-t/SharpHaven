using System;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class RootWidget : Widget
	{
		public RootWidget(IWidgetHost host) : base(host)
		{
		}

		public event Action<KeyboardKeyEventArgs> UnhandledKeyDown;
		public event Action<KeyboardKeyEventArgs> UnhandledKeyUp;

		protected override bool OnKeyDown(KeyboardKeyEventArgs e)
		{
			UnhandledKeyDown.Raise(e);
			return true;
		}

		protected override bool OnKeyUp(KeyboardKeyEventArgs e)
		{
			UnhandledKeyUp.Raise(e);
			return true;
		}
	}
}
