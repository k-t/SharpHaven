namespace MonoHaven.UI.Remote
{
	public class ServerImage : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Image(parent.Widget);
			if (args.Length > 0)
			{
				widget.Drawable = App.Instance.Resources.GetTexture((string)args[0]);
				widget.SetSize(widget.Drawable.Width, widget.Drawable.Height);
			}
			return new ServerImage(id, parent, widget);
		}

		public ServerImage(ushort id, ServerWidget parent, Image widget)
			: base(id, parent, widget)
		{
		}
	}
}
