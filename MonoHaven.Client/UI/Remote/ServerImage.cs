#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerImage : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var resName = (string)args[0];

			var widget = new Image(parent.Widget);
			widget.Drawable = App.Instance.Resources.GetTexture(resName);
			widget.SetSize(widget.Drawable.Width, widget.Drawable.Height);
			return new ServerImage(id, parent, widget);
		}

		public ServerImage(ushort id, ServerWidget parent, Image widget)
			: base(id, parent, widget)
		{
		}
	}
}
