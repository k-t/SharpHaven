#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
			this.widget.MapClicked += OnMapClicked;

		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "move")
			{
				widget.WorldPoint = (Point)args[0];
				return;
			}
			base.ReceiveMessage(message, args);
		}

		private void OnMapClicked(MapClickEventArgs args)
		{
			var button = ServerInput.ToServerButton(args.Button);
			if (args.Gob != null)
				SendMessage("click", args.ScreenPoint, args.MapPoint, button, 0,
					args.Gob.Id, args.Gob.Position);
			else
				SendMessage("click", args.ScreenPoint, args.MapPoint, button, 0);
		}
	}
}
