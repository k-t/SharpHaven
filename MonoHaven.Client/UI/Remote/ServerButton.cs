namespace MonoHaven.UI.Remote
{
	public class ServerButton : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var button = new Button(parent.Widget, (int)args[0]);
			button.Text = (string)args[1];
			return new ServerButton(id, parent, button);
		}

		public ServerButton(ushort id, ServerWidget parent, Button widget)
			: base(id, parent, widget)
		{
			widget.Clicked += () => SendMessage("activate");
		}
	}
}
