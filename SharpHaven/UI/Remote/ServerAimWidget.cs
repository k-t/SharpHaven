using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerAimWidget : ServerWidget
	{
		private AimWidget widget;

		public ServerAimWidget(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("aim", UpdateValue);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerAimWidget(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			widget = Session.Screen.Aim;
			widget.Value = 0;
			widget.Visible = true;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
		}

		private void UpdateValue(object[] args)
		{
			widget.Value = (int)args[0];
		}
	}
}