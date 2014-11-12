namespace MonoHaven.UI.Remote
{
	public class ImageAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var widget = new Image(parent);
			if (args.Length > 0)
			{
				widget.Drawable = App.Instance.Resources.GetTexture((string)args[0]);
				widget.SetSize(widget.Drawable.Width, widget.Drawable.Height);
			}
			return widget;
		}
	}
}
