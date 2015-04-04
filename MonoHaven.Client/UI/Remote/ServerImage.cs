using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;
using Image = MonoHaven.UI.Widgets.Image;

namespace MonoHaven.UI.Remote
{
	public class ServerImage : ServerWidget
	{
		private Image widget;

		public ServerImage(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("ch", SetImage);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerImage(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var resName = (string)args[0];
			var handleClick = args.Length > 2 && (int)args[2] != 0;

			widget = new Image(Parent.Widget);
			widget.Move(position);
			widget.Drawable = App.Resources.Get<Drawable>(resName);
			widget.Resize(widget.Drawable.Size);

			if (handleClick)
				widget.Click += OnClick;
			
		}

		private void SetImage(object[] args)
		{
			widget.Drawable = App.Resources.Get<Drawable>((string)args[0]);
		}

		private void OnClick(MouseButtonEvent e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("click", e.Position, button, mods);
		}
	}
}
