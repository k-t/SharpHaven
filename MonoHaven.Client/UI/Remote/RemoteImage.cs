namespace MonoHaven.UI.Remote
{
	public class RemoteImage : RemoteWidget
	{
		private readonly Image widget;

		public RemoteImage(int id, RemoteWidget parent, object[] args)
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
