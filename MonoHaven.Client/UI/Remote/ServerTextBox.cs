using System.Drawing;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

namespace MonoHaven.UI.Remote
{
	public class ServerTextBox : ServerWidget
	{
		private TextBox widget;

		public ServerTextBox(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("settext", SetText);
			SetHandler("get", Get);
			SetHandler("pw", SetPasswordChar);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerTextBox(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			var size = (Point)args[0];
			var text = (string)args[1];

			widget = new TextBox(Parent.Widget);
			widget.Resize(size.X, size.Y);
			widget.Text = text;
			widget.KeyDown += HandleKeyDown;
		}

		private void SetPasswordChar(object[] args)
		{
			widget.PasswordChar = (int)args[0] == 1 ? '*' : (char?)null;
		}

		private void Get(object[] args)
		{
			SendMessage("text", widget.Text);
		}

		private void SetText(object[] args)
		{
			widget.Text = (string)args[0];
		}

		private void HandleKeyDown(KeyEvent e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					SendMessage("activate", widget.Text);
					e.Handled = true;
					break;
			}
		}
	}
}
