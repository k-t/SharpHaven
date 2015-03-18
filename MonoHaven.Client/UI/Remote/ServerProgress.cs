using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerProgress : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var value = (int)args[0];

			var widget = new Progress(parent.Widget);
			widget.Value = value;
			return new ServerProgress(id, parent, widget);
		}

		private readonly Progress widget;

		public ServerProgress(ushort id, ServerWidget parent, Progress widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "p")
				widget.Value = (int)args[0];
			else
				base.ReceiveMessage(message, args);
		}
	}
}
