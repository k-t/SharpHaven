#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerSpeedget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var cur = (int)args[0];
			var max = (int)args[1];

			var widget = new Speedget(parent.Widget);
			widget.CurrentSpeed = cur;
			widget.MaxSpeed = max;
			return new ServerSpeedget(id, parent, widget);
		}

		private readonly Speedget widget;

		public ServerSpeedget(ushort id, ServerWidget parent, Speedget widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.SpeedSelected += value => SendMessage("set", value);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "cur")
				widget.CurrentSpeed = (int)args[0];
			else if (message == "max")
				widget.MaxSpeed = (int)args[0];
			else
				base.ReceiveMessage(message, args);
		}
	}
}
