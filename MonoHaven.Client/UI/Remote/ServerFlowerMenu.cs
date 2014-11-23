#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Linq;

namespace MonoHaven.UI.Remote
{
	public class ServerFlowerMenu : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var options = args.OfType<string>();
			var widget = new FlowerMenu(parent.Widget, options);
			return new ServerFlowerMenu(id, parent, widget);
		}

		private readonly FlowerMenu widget;

		public ServerFlowerMenu(ushort id, ServerWidget parent, FlowerMenu widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Selected += n => SendMessage("cl", n);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "cancel")
				widget.Remove();
			else
				base.ReceiveMessage(message, args);
		}
	}
}
