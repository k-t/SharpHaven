using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerItemWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			int i = 3;

			var resId = (int)args[0];
			var q = (int)args[1];
			var off = (int)args[2] != 0 ? (Point)args[i++] : Point.Empty;
			var tooltip = args.Length > i ? (string)args[i++] : string.Empty;
			var num = args.Length > i ? (int)args[i] : -1;

			var image = parent.Session.GetImage(resId);
			var widget = new ItemWidget(parent.Widget, image);
			return new ServerItemWidget(id, parent, widget);
		}

		public ServerItemWidget(ushort id, ServerWidget parent, ItemWidget widget)
			: base(id, parent, widget)
		{
		}
	}
}
