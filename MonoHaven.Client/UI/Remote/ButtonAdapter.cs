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
	}
}
