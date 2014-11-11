using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class ImageController : Controller
	{
		private readonly Image widget;

		private ImageController(int id, GameSession session, Image widget)
			: base(id, session)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, GameSession session, Controller parent, object[] args)
		{
			var widget = new Image(parent.Widget);
			if (args.Length > 0)
			{
				widget.Drawable = App.Instance.Resources.GetTexture((string)args[0]);
				widget.SetSize(widget.Drawable.Width, widget.Drawable.Height);
			}
			return new ImageController(id, session, widget);
		}
	}
}
