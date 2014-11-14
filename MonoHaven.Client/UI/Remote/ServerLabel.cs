namespace MonoHaven.UI.Remote
{
	public class ServerLabel : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget, Fonts.Text);
			widget.Text = (string)args[0];
			if (args.Length > 1)
				widget.SetSize((int)args[1], widget.Height);
			return new ServerLabel(id, parent, widget);
		}

		public ServerLabel(ushort id, ServerWidget parent, Label widget)
			: base(id, parent, widget)
		{
		}
	}
}
