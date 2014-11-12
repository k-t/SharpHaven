namespace MonoHaven.UI.Remote
{
	public class ImageButtonAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var defaultImage = args.Length > 0 ? (string)args[0] : null;
			var pressedImage = args.Length > 1 ? (string)args[1] : defaultImage;

			var widget = new ImageButton(parent);
			widget.Image = App.Instance.Resources.GetTexture(defaultImage);
			widget.PressedImage = App.Instance.Resources.GetTexture(pressedImage);
			widget.SetSize(widget.Image.Width, widget.Image.Height);
			return widget;
		}
	}
}
