using System;
using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class WindowAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var size = (Point)args[0];
			var caption = args.Length > 1 ? (string)args[1] : "";
			var window = new Window(parent, caption);
			window.SetSize(size.X, size.Y);
			return window;
		}

		public override void HandleMessage(Widget widget, string message, object[] args)
		{
			var window = (Window)widget;
			if (message == "pack")
				window.Pack();
			else
				base.HandleMessage(widget, message, args);
		}

		public override void SetEventHandler(Widget widget, Action<string, object[]> handler)
		{
			var window = (Window)widget;
			window.Closed += () => handler("close", null);
		}
	}
}
