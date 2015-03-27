using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerImage : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var resName = (string)args[0];
			var handleClick = args.Length > 2 && (int)args[2] != 0;

			var widget = new Image(parent.Widget);
			widget.Drawable = App.Resources.Get<Drawable>(resName);
			widget.Resize(widget.Drawable.Size);
			return new ServerImage(id, parent, widget, handleClick);
		}

		private readonly Image widget;

		public ServerImage(ushort id, ServerWidget parent, Image widget, bool handleClick)
			: base(id, parent, widget)
		{
			this.widget = widget;
			if (handleClick)
				this.widget.Click += OnClick;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "ch")
				widget.Drawable = App.Resources.Get<Drawable>((string)args[0]);
			else
				base.ReceiveMessage(message, args);
		}

		private void OnClick(MouseButtonEvent e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("click", e.Position, button, mods);
		}
	}
}
