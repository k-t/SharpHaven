using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerMapView : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var size = (Point)args[0];
			var worldpos = (Point)args[1];
			var playerId = args.Length > 2 ? (int)args[2] : -1;

			var widget = new MapView(parent.Widget, parent.Session.State, worldpos, playerId);
			widget.SetSize(size.X, size.Y);
			return new ServerMapView(id, parent, widget);
		}

		private readonly MapView widget;

		public ServerMapView(ushort id, ServerWidget parent, MapView widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Clicked += OnClick;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "move")
			{
				widget.WorldCoord = (Point)args[0];
				return;
			}
			base.ReceiveMessage(message, args);
		}

		private void OnClick(Point sc, Point mc)
		{
			SendMessage("click", sc, mc, 1, 0);
		}
	}
}
