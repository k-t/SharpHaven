using System.Drawing;
using Microsoft.Win32;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

namespace MonoHaven.UI.Remote
{
	public class ServerMapView : ServerWidget
	{
		private MapView widget;

		public ServerMapView(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("move", SetWorldPosition);
			SetHandler("place", Place);
			SetHandler("unplace", Unplace);
			SetHandler("polowner", ShowOverlayOwner);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerMapView(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			//var size = (Point)args[0];
			var worldpos = (Point)args[1];
			var playerId = args.Length > 2 ? (int)args[2] : -1;

			Session.State.WorldPosition = worldpos;

			widget = Session.State.Screen.MapView;
			widget.Visible = true;
			widget.PlayerId = playerId;
			widget.State = Session.State;
			widget.MapClick += OnMapClick;
			widget.Placed += OnPlaced;
			widget.ItemDrop += OnItemDrop;
			widget.ItemInteract += OnItemInteract;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
			widget.State = null;
		}

		private void SetWorldPosition(object[] args)
		{
			Session.State.WorldPosition = (Point)args[0];
		}

		private void Place(object[] args)
		{
			var resName = (string)args[0];
			var resVersion = (int)args[1];
			var snapToTile = (int)args[2] != 0;
			var radius = args.Length > 3 ? (int?)args[3] : null;
			var sprite = App.Resources.GetSprite(resName);
			widget.Place(sprite, snapToTile, radius);
		}

		private void Unplace(object[] args)
		{
			widget.Unplace();
		}

		private void ShowOverlayOwner(object[] args)
		{
			var owner = (string)args[0];
			widget.ShowOverlayOwner(owner);
		}

		private void OnMapClick(MapClickEvent e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			if (e.Gob != null)
				SendMessage("click", e.ScreenCoord, e.MapCoord, button, mods, e.Gob.Id, e.Gob.Position);
			else
				SendMessage("click", e.ScreenCoord, e.MapCoord, button, mods);
		}

		private void OnPlaced(MapPlaceEvent e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			SendMessage("place", e.MapCoord, button, mods);
		}

		private void OnItemDrop(KeyModifiers mods)
		{
			SendMessage("drop", ServerInput.ToServerModifiers(mods));
		}

		private void OnItemInteract(MapClickEvent e)
		{
			var mods = ServerInput.ToServerModifiers(e.Modifiers);
			if (e.Gob != null)
				SendMessage("itemact", e.ScreenCoord, e.MapCoord, mods, e.Gob.Id, e.Gob.Position);
			else
				SendMessage("itemact", e.ScreenCoord, e.MapCoord, mods);
		}
	}
}
