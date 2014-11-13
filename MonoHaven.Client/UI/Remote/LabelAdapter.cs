namespace MonoHaven.UI.Remote
{
	public class LabelAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var label = new Label(parent, Fonts.Text);
			label.Text = (string)args[0];
			if (args.Length > 1)
				label.SetSize((int)args[1], label.Height);
			return label;
		}
	}
}
