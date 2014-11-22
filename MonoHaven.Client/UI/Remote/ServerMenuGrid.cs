#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Label(parent.Widget);
			widget.Text = "MENU";
			return new ServerMenuGrid(id, parent, widget);
		}

		public ServerMenuGrid(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
