using System.Drawing;
using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerButton : ServerWidget
	{
		private Button widget;

		public ServerButton(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerButton(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			var width = (int)args[0];
			var text = (string)args[1];

			widget = new Button(Parent.Widget, width);
			widget.Move(position);
			widget.Text = text;
			widget.Click += () => SendMessage("activate");
		}
	}
}
