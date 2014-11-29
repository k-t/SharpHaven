using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerContainer : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var widget = new Container(parent.Widget);
			widget.Resize(size.X, size.Y);
			return new ServerContainer(id, parent, widget);
		}

		public ServerContainer(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
