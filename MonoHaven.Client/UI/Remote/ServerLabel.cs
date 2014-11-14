namespace MonoHaven.UI.Remote
{
	public class ServerLabel : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var text = (string)args[0];
			var width = args.Length > 1 ? (int?)args[1] : null;

			var widget = new Label(parent.Widget, Fonts.Text);
			widget.Text = text;
			if (width.HasValue)
				widget.SetSize(width.Value, widget.Height);
			return new ServerLabel(id, parent, widget);
		}

		public ServerLabel(ushort id, ServerWidget parent, Label widget)
			: base(id, parent, widget)
		{
		}
	}
}
