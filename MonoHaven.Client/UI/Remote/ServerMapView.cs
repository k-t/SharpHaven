using System.Drawing;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

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
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			if (e.Gob != null)
				SendMessage("click", e.ScreenCoord, e.MapCoord, button, mods, e.Gob.Id, e.Gob.Position);
			else
				SendMessage("click", e.ScreenCoord, e.MapCoord, button, mods);
		}

		private void OnPlaced(MapPlaceEventArgs e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("place", e.MapCoord, button, mods);
		}

		private void OnItemDrop(KeyModifiers mods)
		{
			SendMessage("drop", ServerInput.ToServerModifiers(mods));
		}

		private void OnItemInteract(MapClickEventArgs e)
		{
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			if (e.Gob != null)
				SendMessage("itemact", e.ScreenCoord, e.MapCoord, mods, e.Gob.Id, e.Gob.Position);
			else
				SendMessage("itemact", e.ScreenCoord, e.MapCoord, mods);
		}
	}
}
