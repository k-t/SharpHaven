#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

		private readonly TextBox widget;

		public ServerTextBox(ushort id, ServerWidget parent, TextBox widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Done += text => SendMessage("activate", text);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "settext")
				widget.Text = (string)args[0];
			else if (message == "get")
				SendMessage("text", widget.Text);
			else if (message == "pw")
				widget.PasswordChar = (int)args[0] == 1 ? '*' : (char?)null;
			else
				base.ReceiveMessage(message, args);
		}
	}
}
