#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerItemWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			int i = 3;

			var resId = (int)args[0];
			var q = (int)args[1];
			var off = (int)args[2] != 0 ? (Point)args[i++] : Point.Empty;
			var tooltip = args.Length > i ? (string)args[i++] : string.Empty;
			var num = args.Length > i ? (int)args[i] : -1;

			var res = parent.Session.GetResource(resId);
			var widget = new ItemWidget(parent.Widget, res);
			return new ServerItemWidget(id, parent, widget);
		}

		public ServerItemWidget(ushort id, ServerWidget parent, ItemWidget widget)
			: base(id, parent, widget)
		{
		}
	}
}
