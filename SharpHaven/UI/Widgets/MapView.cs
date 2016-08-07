using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class MapView : Widget, IItemDropTarget
	{
		private static readonly Drawable circle;
		private static readonly Drawable overlay;
		private static readonly List<Tuple<Coord2d, Drawable>> overlayBorders;
		private static readonly Color[] overlayColors;

		private bool dragging;
		private Coord2d dragPosition;
		private Coord2d dragCameraOffset;
		private Gob placeGob;
		private int placeRadius;
		private bool placeOnTile;
		private string ownerName;
		private TextLine ownerNameText;
		private DateTime ownerShowTime;

		static MapView()
		{
			circle = App.Resources.Get<Drawable>("gfx/circle");

			var tileset = App.Resources.Get<Tileset>("gfx/tiles/ol/ol");
			var tiles = tileset.GroundTiles.ToList();
			overlay = tiles[0];
			overlayBorders = new List<Tuple<Coord2d, Drawable>> {
				Tuple.Create(new Coord2d(0, -1), tiles[1]),
				Tuple.Create(new Coord2d(0, 1), tiles[2]),
				Tuple.Create(new Coord2d(-1, 0), tiles[3]),
				Tuple.Create(new Coord2d(1, 0), tiles[4])
			};

			overlayColors = new Color[31];
			overlayColors[0] = Color.FromArgb(255, 0, 128);
			overlayColors[1] = Color.FromArgb(0, 0, 255);
			overlayColors[2] = Color.FromArgb(255, 0, 0);
			overlayColors[3] = Color.FromArgb(128, 0, 255);
			overlayColors[16] = Color.FromArgb(0, 255, 0);
			overlayColors[17] = Color.FromArgb(255, 255, 0);
		}

		public MapView(Widget parent) : base(parent)
		{
			IsFocusable = true;

			ownerNameText = new TextLine(Fonts.Create(FontFaces.Serif, 20));
			ownerNameText.TextColor = Color.White;
		}

		public event Action<MapClickEvent> MapClick;
		public event Action<KeyModifiers> ItemDrop;
		public event Action<MapClickEvent> ItemInteract;
		public event Action<MapPlaceEvent> Placed;
		public event Action<Coord2d> GridRequest;

		public int PlayerId
		{
			get;
			set;
		}

		public ClientSession Session
		{
			get;
			set;
		}

		private Coord2d CameraOffset
		{
			get { return Geometry.MapToScreen(Session.WorldPosition); }
			set { Session.WorldPosition = Geometry.ScreenToMap(value); }
		}

		public void Place(ISprite sprite, bool snapToTile, int? radius)
		{
			if (Session == null)
				throw new InvalidOperationException();

			placeOnTile = snapToTile;
			placeRadius = radius ?? -1;
			placeGob = new Gob(-1);
			placeGob.SetSprite(new Delayed<ISprite>(sprite));

			var mc = Geometry.ScreenToMap(ToAbsolute(Host.MousePosition));
			placeGob.Position = placeOnTile ? Geometry.Tilify(mc) : mc;

			Session.Objects.AddLocal(placeGob);
		}

		public void Unplace()
		{
			if (Session == null)
				throw new InvalidOperationException();

			Session.Objects.RemoveLocal(placeGob);
			placeGob = null;
		}

		public void ShowOverlayOwner(string name)
		{
			ownerNameText.Clear();
			if (string.IsNullOrEmpty(name))
			{
				if (string.IsNullOrEmpty(ownerName))
					return;

				ownerNameText.Append("Leaving "+ ownerName);
				ownerShowTime = DateTime.Now;
				ownerName = null;
			}
			else
			{
				ownerName = name;
				ownerNameText.Append("Entering " + name);
				ownerShowTime = DateTime.Now;
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Session == null)
				return;

			RequestMaps();
			DrawTiles(dc);
			DrawScene(dc);

			// draw placed object
			if (placeGob != null && placeRadius > 0)
			{
				var p = ToRelative(Geometry.MapToScreen(placeGob.Position));
				int w = (int)(placeRadius * 4 * Math.Sqrt(0.5)) * 2;
				int h = (int)(placeRadius * 2 * Math.Sqrt(0.5)) * 2;
				dc.SetColor(0, 255, 0, 32);
				dc.Draw(circle, p.X - (w / 2), p.Y - (h / 2), w, h);
				dc.ResetColor();
			}

			// draw owner name
			var span = (DateTime.Now - ownerShowTime).TotalSeconds;
			if (span < 6)
			{
				byte a;
				if (span < 1)
					a = (byte)(255 * span);
				else if (span < 4)
					a = 255;
				else
					a = (byte)((255 * (2 - (span - 4))) / 2);

				ownerNameText.TextColor = Color.FromArgb(a, Color.White);
				dc.Draw(ownerNameText,
					(Width - ownerNameText.TextWidth) / 2,
					(Height - ownerNameText.Font.Height) / 2);
			}
		}

		private void RequestMaps()
		{
			if (PlayerId != -1)
			{
				var player = Session.Objects.Get(PlayerId);
				var ul = Geometry.MapToGrid(player.Position.Add(-500));
				var br = Geometry.MapToGrid(player.Position.Add(500));
				for (int y = ul.Y; y <= br.Y; y++)
					for (int x = ul.X; x <= br.X; x++)
						GridRequest.Raise(new Coord2d(x, y));
			}
		}

		private void DrawTiles(DrawingContext g)
		{
			// get tile in the center
			var center = Geometry.ScreenToTile(CameraOffset);
			// how much tiles fit onto screen vertically and horizontally
			var h = Width / (GameScene.ScreenTileWidth * 2);
			var v = Height / (GameScene.ScreenTileHeight * 2);

			// draw all tiles around the center one
			for (int x = -(h + 4); x <= h + 2; x++)
				for (int y = -(v + 3); y <= v + 2; y++)
					for (int i = 0; i < 2; i++)
					{
						int tx = center.X + x + y;
						int ty = center.Y + y - x - i;
						DrawTile(g, tx, ty);
					}
		}

		private void DrawTile(DrawingContext g, int tx, int ty)
		{
			var tile = Session.Map.GetTile(tx, ty);
			if (tile == null)
				return;
			// determine screen position
			var sc = ToRelative(Geometry.TileToScreen(tx, ty));
			// draw tile itself
			g.Draw(tile.Texture, sc.X, sc.Y);
			// draw transitions
			foreach (var trans in tile.Transitions)
				g.Draw(trans, sc.X, sc.Y);

			// draw overlays
			foreach (var overlayIndex in tile.Overlays)
			{
				var color = overlayColors[overlayIndex];
				g.SetColor(color);
				g.Draw(overlay, sc.X, sc.Y);
				foreach (var border in overlayBorders)
				{
					int dx = border.Item1.X;
					int dy = border.Item1.Y;
					var btile = Session.Map.GetTile(tx + dx, ty + dy);
					if (btile != null && !btile.Overlays.Contains(overlayIndex))
						g.Draw(border.Item2, sc.X, sc.Y);
				}
				g.ResetColor();
			}
			foreach (var mapOverlay in Session.Map.Overlays.Where(x => x.Bounds.Contains(tx, ty)))
			{
				var color = overlayColors[mapOverlay.Index];
				g.SetColor(color);
				g.Draw(overlay, sc.X, sc.Y);
				foreach (var border in overlayBorders)
				{
					int dx = border.Item1.X;
					int dy = border.Item1.Y;
					if (!mapOverlay.Bounds.Contains(tx + dx, ty + dy))
						g.Draw(border.Item2, sc.X, sc.Y);
				}
				g.ResetColor();
			}
		}

		private void DrawScene(DrawingContext g)
		{
			Session.Scene.Draw(g, Width / 2 - CameraOffset.X, Height / 2 - CameraOffset.Y);
		}

		protected override void OnKeyDown(KeyEvent e)
		{
			e.Handled = true;
			switch (e.Key)
			{
				case Key.Up:
					MoveCamera(0, -50);
					break;
				case Key.Down:
					MoveCamera(0, 50);
					break;
				case Key.Left:
					MoveCamera(-50, 0);
					break;
				case Key.Right:
					MoveCamera(50, 0);
					break;
				case Key.Home:
				case Key.Keypad7:
					if (PlayerId != -1)
					{
						var player = Session.Objects.Get(PlayerId);
						Session.WorldPosition = player.Position;
					}
					break;
				default:
					e.Handled = false;
					base.OnKeyDown(e);
					break;
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (Session == null)
				return;

			var sc = ToAbsolute(e.Position);
			var mc = Geometry.ScreenToMap(sc);
			var gob = Session.Scene.GetObjectAt(sc);

			if (e.Button == MouseButton.Middle)
			{
				Host.GrabMouse(this);
				dragging = true;
				dragPosition = e.Position;
				dragCameraOffset = CameraOffset;
			}
			else if (placeGob != null)
			{
				Placed.Raise(new MapPlaceEvent(e, placeGob.Position));
				Session.Objects.RemoveLocal(placeGob);
				placeGob = null;
			}
			else
			{
				MapClick.Raise(new MapClickEvent(e, mc, e.Position, gob));
			}
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (Session == null)
				return;

			if (e.Button == MouseButton.Middle)
			{
				Host.ReleaseMouse();
				dragging = false;
			}
			e.Handled = true;
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (Session == null)
				return;

			if (dragging)
			{
				CameraOffset = dragCameraOffset.Add(dragPosition.Sub(e.Position));
			}
			if (placeGob != null)
			{
				var mc = Geometry.ScreenToMap(ToAbsolute(e.Position));
				var snap = placeOnTile ^ e.Modifiers.HasShift();
				placeGob.Position = snap ? Geometry.Tilify(mc) : mc;
			}
		}

		protected override void OnDispose()
		{
			ownerNameText.Dispose();
		}

		private void MoveCamera(int deltaX, int deltaY)
		{
			CameraOffset = CameraOffset.Add(deltaX, deltaY);
		}

		/// <summary>
		/// Converts absolute screen coordinate to a relative to the current viewport.
		/// </summary>
		private Coord2d ToRelative(Coord2d abs)
		{
			return new Coord2d(
				abs.X + Width / 2 - CameraOffset.X,
				abs.Y + Height / 2 - CameraOffset.Y);
		}

		/// <summary>
		/// Converts relative screen coordinate to absolute.
		/// </summary>
		private Coord2d ToAbsolute(Coord2d rel)
		{
			return new Coord2d(
				rel.X - Width / 2 + CameraOffset.X,
				rel.Y - Height / 2 + CameraOffset.Y);
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Coord2d p, Coord2d ul, KeyModifiers mods)
		{
			ItemDrop.Raise(mods);
			return true;
		}

		bool IItemDropTarget.Interact(Coord2d p, Coord2d ul, KeyModifiers mods)
		{
			if (Session == null)
				return false;

			var sc = ToAbsolute(p);
			var mc = Geometry.ScreenToMap(sc);
			var gob = Session.Scene.GetObjectAt(sc);
			ItemInteract.Raise(new MapClickEvent(0, mods, mc, p, gob));
			return true;
		}

		#endregion
	}
}
