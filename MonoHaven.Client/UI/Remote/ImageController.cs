namespace MonoHaven.UI.Remote
{
	public class ImageController : Controller
	{
		private readonly Image widget;

		public ImageController(int id, Controller parent, object[] args)
			: base(id)
		{
			widget = new Image(parent.Widget);
			if (args.Length > 0)
				widget.Drawable = App.Instance.Resources.GetTexture((string)args[0]);
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
