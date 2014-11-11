namespace MonoHaven.UI.Remote
{
	public class CharlistController : Controller
	{
		private readonly Charlist widget;

		private CharlistController(int id, Charlist widget) : base(id)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public override void HandleMessage(string message, object[] args)
		{
			if (message == "add")
			{
				var name = (string)args[0];
				widget.AddChar(name);
				return;
			}
			base.HandleMessage(message, args);
		}

		public static Controller Create(int id, Controller parent, object[] args)
		{
			int height = args.Length > 0 ? (int)args[0] : 0;
			return new CharlistController(id, new Charlist(parent.Widget, height));
		}
	}
}
