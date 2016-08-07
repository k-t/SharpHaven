using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerImageButton : ServerWidget
	{
		private ImageButton widget;

		public ServerImageButton(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerImageButton(id, parent);
		}

		protected override void OnInit(Coord2D position, object[] args)
		{
			var defaultImage = args.Length > 0 ? (string)args[0] : null;
			var pressedImage = args.Length > 1 ? (string)args[1] : defaultImage;

			widget = new ImageButton(Parent.Widget);
			widget.Move(position);
			widget.Image = App.Resources.Get<Drawable>(defaultImage);
			widget.PressedImage = App.Resources.Get<Drawable>(pressedImage);
			widget.Resize(widget.Image.Size);
			widget.Click += () => SendMessage("activate");
		}
	}
}
