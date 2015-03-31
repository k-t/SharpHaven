using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapView : Widget, IDropTarget
	{
		private static readonly Drawable circle;
		private static readonly Drawable ol;
		private static readonly Drawable olTop;
		private static readonly Drawable olBottom;
		private static readonly Drawable olLeft;
		private static readonly Drawable olRight;

		private readonly GameState gstate;
		private readonly int playerId;
		private bool dragging;
		private Point dragPosition;
		private Point dragCameraOffset;
		private Gob placeGob;
		private int placeRadius;
		private bool placeOnTile;

		static MapView()
		{
			circle = App.Resources.Get<Drawable>("custom/ui/circle");
			ol = App.Resources.Get<Drawable>("custom/gfx/ol");
			olTop = App.Resources.Get<Drawable>("custom/gfx/ol-top");
			olBottom = App.Resources.Get<Drawable>("custom/gfx/ol-bottom");
			olLeft = App.Resources.Get<Drawable>("custom/gfx/ol-left");
			olRight = App.Resources.Get<Drawable>("custom/gfx/ol-right");
		}

		public MapView(Widget parent, GameState gstate, Point mc, int playerId)
			: base(parent)
		{
			IsFocusable = true;

			this.gstate = gstate;
			this.gstate.WorldPosition = mc;
			this.playerId = playerId;
		}

		public event Action<MapClickEventArgs> MapClick;
		public event Action<KeyModifiers> ItemDrop;
		public event Action<MapClickEventArgs> ItemInteract;
		public event Action<MapPlaceEventArgs> Placed;

		private Point CameraOffset
		{
			get { return Geometry.MapToScreen(gstate.WorldPosition); }
			set { gstate.WorldPosition = Geometry.ScreenToMap(value); }
		}

		public void Place(ISprite sprite, bool snapToTile, int? radius)
		{
			placeOnTile = snapToTile;
			placeRadius = radius ?? -1;
			placeGob = new Gob(-1);
			placeGob.SetSprite(new Delayed<ISprite>(sprite));

			var mc = Geometry.ScreenToMap(ToAbsolute(Host.MousePosition));
			placeGob.Position = placeOnTile ? Geometry.Tilify(mc) : mc;

			gstate.Objects.AddLocal(placeGob);
		}

		public void Unplace()
		{
			gstate.Objects.RemoveLocal(placeGob);
			placeGob = null;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			RequestMaps();
			DrawTiles(dc);
			DrawScene(dc);

			if (placeGob != null && placeRadius > 0)
			{
				var p = ToRelative(Geometry.MapToScreen(placeGob.Position));
				int w = (int)(placeRadius * 4 * Math.Sqrt(0.5)) * 2;
				int h = (int)(placeRadius * 2 * Math.Sqrt(0.5)) * 2;
				dc.SetColor(0, 255, 0, 32);
				dc.Draw(circle, p.X - (w / 2), p.Y - (h / 2), w, h);
				dc.ResetColor();
			}
		}

		private void RequestMaps()
		{
			if (playerId != -1)
			{
				var player = gstate.Objects.Get(playerId);
				var ul = Geometry.MapToGrid(player.Position.Add(-500));
				var br = Geometry.MapToGrid(player.Position.Add(500));
				for (int y = ul.Y; y <= br.Y; y++)
					for (int x = ul.X; x <= br.X; x++)
						gstate.Map.Request(x, y);
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
						var sc = ToRelative(Geometry.TileToScreen(tx, ty));
						var tile = gstate.Map.GetTile(tx, ty);
						if (tile != null)
						{
							g.Draw(tile.Texture, sc.X, sc.Y);
							foreach (var trans in tile.Transitions)
								g.Draw(trans, sc.X, sc.Y);
							foreach (var overlay in tile.Overlays)
							{
								g.SetColor(overlay);
								g.Draw(ol, sc.X, sc.Y);
								if (!HasOverlay(gstate.Map.GetTile(tx, ty - 1), overlay))
									g.Draw(olTop, sc.X, sc.Y);
								if (!HasOverlay(gstate.Map.GetTile(tx, ty + 1), overlay))
									g.Draw(olBottom, sc.X, sc.Y);
								if (!HasOverlay(gstate.Map.GetTile(tx - 1, ty), overlay))
									g.Draw(olLeft, sc.X, sc.Y);
								if (!HasOverlay(gstate.Map.GetTile(tx + 1, ty), overlay))
									g.Draw(olRight, sc.X, sc.Y);
								g.ResetColor();
							}
						}
					}
		}

		private bool HasOverlay(MapTile tile, Color overlay)
		{
			return tile == null || tile.Overlays.Contains(overlay);
		}

		private void DrawScene(DrawingContext g)
		{
			gstate.Scene.Draw(g, Width / 2 - CameraOffset.X, Height / 2 - CameraOffset.Y);
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
					if (playerId != -1)
					{
						var player = gstate.Objects.Get(playerId);
						gstate.WorldPosition = player.Position;
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
			var sc = ToAbsolute(e.Position);
			var mc = Geometry.ScreenToMap(sc);
			var gob = gstate.Scene.GetObjectAt(sc);

			if (e.Button == MouseButton.Middle)
			{
				Host.GrabMouse(this);
				dragging = true;
				dragPosition = e.Position;
				dragCameraOffset = CameraOffset;
			}
			else if (placeGob != null)
			{
				Placed.Raise(new MapPlaceEventArgs(e, placeGob.Position));
				gstate.Objects.RemoveLocal(placeGob);
				placeGob = null;
			}
			else
			{
				MapClick.Raise(new MapClickEventArgs(e, mc, e.Position, gob));
			}
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Middle)
			{
				Host.ReleaseMouse();
				dragging = false;
			}
			e.Handled = true;
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
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

		#region IDropTarget

		bool IDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			ItemDrop.Raise(mods);
			return true;
		}

		bool IDropTarget.ItemInteract(Point p, Point ul, KeyModifiers mods)
		{
			var sc = ToAbsolute(p);
			var mc = Geometry.ScreenToMap(sc);
			var gob = gstate.Scene.GetObjectAt(sc);
			ItemInteract.Raise(new MapClickEventArgs(0, mods, mc, p, gob));
			return true;
		}

		#endregion
	}
}
