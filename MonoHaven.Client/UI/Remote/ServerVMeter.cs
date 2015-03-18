using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerVMeter : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var amount = (int)args[0];
			var color = args.Length > 4
				? Color.FromArgb((int)args[1], (int)args[2], (int)args[3], (int)args[4])
				: Color.FromArgb((int)args[1], (int)args[2], (int)args[3]);
			
			var widget = new VMeter(parent.Widget);
			widget.Amount = amount;
			widget.Color = color;
			return new ServerVMeter(id, parent, widget);
		}

		private readonly VMeter widget;

		public ServerVMeter(ushort id, ServerWidget parent, VMeter widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "set")
				widget.Amount = (int)args[0];
			else
				base.ReceiveMessage(message, args);
		}
	}
}
