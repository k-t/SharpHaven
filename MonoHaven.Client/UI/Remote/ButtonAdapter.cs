using System;

namespace MonoHaven.UI.Remote
{
	public class ButtonAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var button = new Button(parent, (int)args[0]);
			button.Text = (string)args[1];
			return button;
		}

		public override void SetEventHandler(Widget widget, Action<string, object[]> handler)
		{
			var button = (Button)widget;
			button.Clicked += () => handler("activate", null);
		}
	}
}
