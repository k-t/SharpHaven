#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerLabel : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var text = (string)args[0];
			var width = args.Length > 1 ? (int?)args[1] : null;

			var widget = new Label(parent.Widget, Fonts.Text);
			widget.Text = text;
			if (width.HasValue)
				widget.SetSize(width.Value, widget.Height);
			return new ServerLabel(id, parent, widget);
		}

		public ServerLabel(ushort id, ServerWidget parent, Label widget)
			: base(id, parent, widget)
		{
		}
	}
}
