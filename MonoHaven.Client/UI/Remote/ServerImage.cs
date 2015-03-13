using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerImage : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var resName = (string)args[0];

			var widget = new Image(parent.Widget);
			widget.Drawable = App.Resources.GetImage(resName);
			widget.Resize(widget.Drawable.Size);
			return new ServerImage(id, parent, widget);
		}

		public ServerImage(ushort id, ServerWidget parent, Image widget)
			: base(id, parent, widget)
		{
		}
	}
}
