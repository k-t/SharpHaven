namespace MonoHaven.UI.Remote
{
	public class ImageController : Controller
	{
		private readonly Image widget;

		private ImageController(int id, Image widget)
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
			var widget = new Image(parent.Widget);
			if (args.Length > 0)
				widget.Drawable = App.Instance.Resources.GetTexture((string)args[0]);

			return new ImageController(id, widget);
		}
	}
}
