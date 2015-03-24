using System.Drawing;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;

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
			widget.Resize(size.X, size.Y);
			return new ServerMapView(id, parent, widget);
		}

		private readonly MapView widget;

		public ServerMapView(ushort id, ServerWidget parent, MapView widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.MapClicked += OnMapClicked;
			this.widget.Placed += OnPlaced;
			this.widget.ItemDrop += OnItemDrop;
			this.widget.ItemInteract += OnItemInteract;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "move")
				widget.WorldPoint = (Point)args[0];
			else if (message == "place")
			{
				var resName = (string)args[0];
				var resVersion = (int)args[1];
				var snapToTile = (int)args[2] != 0;
				var radius = args.Length > 3 ? (int?)args[3] : null;
				var sprite = App.Resources.GetSprite(resName);
				widget.Place(sprite, snapToTile, radius);
			}
			else if (message == "unplace")
				widget.Unplace();
			else
				base.ReceiveMessage(message, args);
		}

		private void OnMapClicked(MapClickEventArgs e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			if (e.Gob != null)
				SendMessage("click", e.ScreenPoint, e.MapPoint, button,
					(int)e.Modifiers, e.Gob.Id, e.Gob.Position);
			else
				SendMessage("click", e.ScreenPoint, e.MapPoint, button,
					(int)e.Modifiers);
		}

		private void OnPlaced(MapPlaceEventArgs e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			SendMessage("place", e.Point, button, (int)e.Modifiers);
		}

		private void OnItemDrop(Input.KeyModifiers mods)
		{
			SendMessage("drop", (int)mods);
		}

		private void OnItemInteract(MapClickEventArgs e)
		{
			if (e.Gob != null)
				SendMessage("itemact", e.ScreenPoint, e.MapPoint, (int)e.Modifiers,
					e.Gob.Id, e.Gob.Position);
			else
				SendMessage("itemact", e.ScreenPoint, e.MapPoint, (int)e.Modifiers);
		}
	}
}
