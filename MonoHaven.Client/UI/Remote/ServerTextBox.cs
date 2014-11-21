using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerTextBox : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var text = (string)args[1];

			var widget = new TextBox(parent.Widget);
			widget.SetSize(size.X, size.Y);
			widget.Text = text;
			return new ServerTextBox(id, parent, widget);
		}

		public ServerTextBox(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
