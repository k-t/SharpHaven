using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Graphics.Text;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapView : Widget, IItemDropTarget
	{
		private static readonly Drawable circle;
		private static readonly Drawable overlay;
		private static readonly List<Tuple<Point, Drawable>> overlayBorders;

		private bool dragging;
		private Point dragPosition;
		private Point dragCameraOffset;
		private Gob placeGob;
		private int placeRadius;
		private bool placeOnTile;
		private string ownerName;
		private TextLine ownerNameText;
		private DateTime ownerShowTime;

		static MapView()
		{
			circle = App.Resources.Get<Drawable>("custom/ui/circle");

			var tileset = App.Resources.Get<Tileset>("custom/gfx/tiles/ol/ol");
			var tiles = tileset.GroundTiles.ToList();
			overlay = tiles[0];
			overlayBorders = new List<Tuple<Point, Drawable>> {
				Tuple.Create(new Point(0, -1), tiles[1]),
				Tuple.Create(new Point(0, 1), tiles[2]),
				Tuple.Create(new Point(-1, 0), tiles[3]),
				Tuple.Create(new Point(1, 0), tiles[4])
			};
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

		public int PlayerId
		{
			get;
			set;
		}

		public GameState State
		{
			get;
			set;
		}

		private Point CameraOffset
		{
			get { return Geometry.MapToScreen(State.WorldPosition); }
			set { State.WorldPosition = Geometry.ScreenToMap(value); }
		}

		public void Place(ISprite sprite, bool snapToTile, int? radius)
		{
			if (State == null)
				throw new InvalidOperationException();

			placeOnTile = snapToTile;
			placeRadius = radius ?? -1;
			placeGob = new Gob(-1);
			placeGob.SetSprite(new Delayed<ISprite>(sprite));

			var mc = Geometry.ScreenToMap(ToAbsolute(Host.MousePosition));
			placeGob.Position = placeOnTile ? Geometry.Tilify(mc) : mc;

			State.Objects.AddLocal(placeGob);
		}

		public void Unplace()
		{
			if (State == null)
				throw new InvalidOperationException();

			State.Objects.RemoveLocal(placeGob);
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
			if (State == null)
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
				var player = State.Objects.Get(PlayerId);
				var ul = Geometry.MapToGrid(player.Position.Add(-500));
				var br = Geometry.MapToGrid(player.Position.Add(500));
				for (int y = ul.Y; y <= br.Y; y++)
					for (int x = ul.X; x <= br.X; x++)
						State.Map.Request(x, y);
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
			var tile = State.Map.GetTile(tx, ty);
			if (tile == null)
				return;
			// determine screen position
			var sc = ToRelative(Geometry.TileToScreen(tx, ty));
			// draw tile itself
			g.Draw(tile.Texture, sc.X, sc.Y);
			// draw transitions
			foreach (var trans in tile.Transitions)
				g.Draw(trans, sc.X, sc.Y);
			// draw overlay
			foreach (var color in tile.Overlays)
			{
				g.SetColor(color);
				g.Draw(overlay, sc.X, sc.Y);
				foreach (var border in overlayBorders)
				{
					int dx = border.Item1.X;
					int dy = border.Item1.Y;
					var btile = State.Map.GetTile(tx + dx, ty + dy);
					if (btile != null && !btile.Overlays.Contains(color))
						g.Draw(border.Item2, sc.X, sc.Y);
				}
				g.ResetColor();
			}
		}

		private void DrawScene(DrawingContext g)
		{
			State.Scene.Draw(g, Width / 2 - CameraOffset.X, Height / 2 - CameraOffset.Y);
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
						var player = State.Objects.Get(PlayerId);
						State.WorldPosition = player.Position;
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
			if (State == null)
				return;

			var sc = ToAbsolute(e.Position);
			var mc = Geometry.ScreenToMap(sc);
			var gob = State.Scene.GetObjectAt(sc);

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
				State.Objects.RemoveLocal(placeGob);
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
			if (State == null)
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
			if (State == null)
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
		private Point ToRelative(Point abs)
		{
			return new Point(
				abs.X + Width / 2 - CameraOffset.X,
				abs.Y + Height / 2 - CameraOffset.Y);
		}

		/// <summary>
		/// Converts relative screen coordinate to absolute.
		/// </summary>
		private Point ToAbsolute(Point rel)
		{
			return new Point(
				rel.X - Width / 2 + CameraOffset.X,
				rel.Y - Height / 2 + CameraOffset.Y);
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			ItemDrop.Raise(mods);
			return true;
		}

		bool IItemDropTarget.Interact(Point p, Point ul, KeyModifiers mods)
		{
			if (State == null)
				return false;

			var sc = ToAbsolute(p);
			var mc = Geometry.ScreenToMap(sc);
			var gob = State.Scene.GetObjectAt(sc);
			ItemInteract.Raise(new MapClickEvent(0, mods, mc, p, gob));
			return true;
		}

		#endregion
	}
}
