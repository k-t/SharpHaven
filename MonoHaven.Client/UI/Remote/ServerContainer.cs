#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerContainer : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var widget = new Container(parent.Widget);
			widget.SetSize(size.X, size.Y);
			return new ServerContainer(id, parent, widget);
		}

		public ServerContainer(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
