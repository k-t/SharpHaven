using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerLabel : ServerWidget
	{
		private Label widget;

		public ServerLabel(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerLabel(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var text = (string)args[0];
			var width = args.Length > 1 ? (int?)args[1] : null;

			widget = new Label(Parent.Widget, Fonts.Text);
			widget.Move(position);
			widget.AutoSize = true;
			widget.Text = text;
		}
	}
}
