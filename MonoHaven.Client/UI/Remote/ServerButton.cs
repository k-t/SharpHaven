#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerButton : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var width = (int)args[0];
			var text = (string)args[1];

			var button = new Button(parent.Widget, width);
			button.Text = text;
			return new ServerButton(id, parent, button);
		}

		public ServerButton(ushort id, ServerWidget parent, Button widget)
			: base(id, parent, widget)
		{
			widget.Clicked += () => SendMessage("activate");
		}
	}
}
