#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerInventoryWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var widget = new InventoryWidget(parent.Widget);
			widget.SetInventorySize(size);
			return new ServerInventoryWidget(id, parent, widget);
		}

		public ServerInventoryWidget(ushort id, ServerWidget parent, InventoryWidget widget)
			: base(id, parent, widget)
		{
		}
	}
}
