#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI.Remote
{
	public class ServerImageButton : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var defaultImage = args.Length > 0 ? (string)args[0] : null;
			var pressedImage = args.Length > 1 ? (string)args[1] : defaultImage;

			var widget = new ImageButton(parent.Widget);
			widget.Image = App.Instance.Resources.GetTexture(defaultImage);
			widget.PressedImage = App.Instance.Resources.GetTexture(pressedImage);
			widget.SetSize(widget.Image.Width, widget.Image.Height);

			return new ServerImageButton(id, parent, widget);
		}

		public ServerImageButton(ushort id, ServerWidget parent, ImageButton widget)
			: base(id, parent, widget)
		{
			widget.Clicked += () => SendMessage("activate");
		}
	}
}
