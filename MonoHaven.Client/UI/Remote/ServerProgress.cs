using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerProgress : ServerWidget
	{
		private Progress widget;

		public ServerProgress(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("p", Update);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerProgress(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var value = (int)args[0];

			widget = new Progress(Parent.Widget);
			widget.Move(position);
			widget.Value = value;
		}

		private void Update(object[] args)
		{
			widget.Value = (int)args[0];
		}
	}
}
