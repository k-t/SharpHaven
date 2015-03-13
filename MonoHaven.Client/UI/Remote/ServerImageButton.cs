using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerImageButton : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var defaultImage = args.Length > 0 ? (string)args[0] : null;
			var pressedImage = args.Length > 1 ? (string)args[1] : defaultImage;

			var widget = new ImageButton(parent.Widget);
			widget.Image = App.Resources.GetImage(defaultImage, true);
			widget.PressedImage = App.Resources.GetImage(pressedImage);
			widget.Resize(widget.Image.Size);

			return new ServerImageButton(id, parent, widget);
		}

		public ServerImageButton(ushort id, ServerWidget parent, ImageButton widget)
			: base(id, parent, widget)
		{
			widget.Clicked += () => SendMessage("activate");
		}
	}
}
