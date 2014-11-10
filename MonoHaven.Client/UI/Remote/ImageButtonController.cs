namespace MonoHaven.UI.Remote
{
	public class ImageButtonController : Controller
	{
		private readonly ImageButton widget;

		private ImageButtonController(int id, ImageButton widget)
			: base(id)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, Controller parent, object[] args)
		{
			var defaultImage = args.Length > 0 ? (string)args[0] : null;
			var pressedImage = args.Length > 1 ? (string)args[1] : defaultImage;

			var widget = new ImageButton(parent.Widget);
			widget.Image = App.Instance.Resources.GetTexture(defaultImage);
			widget.PressedImage = App.Instance.Resources.GetTexture(pressedImage);
			widget.SetSize(widget.Image.Width, widget.Image.Height);
			return new ImageButtonController(id, widget);
		}
	}
}
