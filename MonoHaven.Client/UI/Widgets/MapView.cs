using System;
using System.Drawing;
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

		static MapView()
		{
			circle = App.Resources.Get<Drawable>("custom/ui/circle");
		}

		private readonly GameState gstate;
		private readonly int playerId;
		private bool dragging;
		private Gob placeGob;
		private int placeRadius;
		private bool placeOnTile;

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

		private Rectangle ScreenBox
		{
			get { return new Rectangle(CameraOffset, Size); }
		}

		public void Place(ISprite sprite, bool snapToTile, int? radius)
		{
			placeOnTile = snapToTile;
			placeRadius = radius ?? -1;
			placeGob = new Gob(-1);
			placeGob.SetSprite(new Delayed<ISprite>(sprite));

			var mc = Geometry.ScreenToMap(Host.MousePosition, ScreenBox);
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
				var p = Geometry.MapToScreen(placeGob.Position, ScreenBox);
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

						var p = Geometry.TileToScreen(tx, ty, ScreenBox);
						p.X -= Geometry.TileWidth * 2;

						var tile = gstate.Map.GetTile(tx, ty);
						if (tile != null)
						{
							g.Draw(tile.Texture, p.X, p.Y);
							foreach (var trans in tile.Transitions)
								g.Draw(trans, p.X, p.Y);
						}
					}
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
					CameraOffset = CameraOffset.Add(0, -50);
					break;
				case Key.Down:
					CameraOffset = CameraOffset.Add(0, 50);
					break;
				case Key.Left:
					CameraOffset = CameraOffset.Add(-50, 0);
					break;
				case Key.Right:
					CameraOffset = CameraOffset.Add(50, 0);
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
			var mc = Geometry.ScreenToMap(e.Position, ScreenBox);
			var gob = gstate.Scene.GetObjectAt(e.Position, ScreenBox);

			if (e.Button == MouseButton.Middle)
			{
				Host.GrabMouse(this);
				dragging = true;
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
				CameraOffset = CameraOffset.Add(-e.DeltaX, -e.DeltaY);
			}
			if (placeGob != null)
			{
				var mc = Geometry.ScreenToMap(e.Position, ScreenBox);
				var snap = placeOnTile ^ e.Modifiers.HasShift();
				placeGob.Position = snap ? Geometry.Tilify(mc) : mc;
			}
		}

		#region IDropTarget

		bool IDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			ItemDrop.Raise(mods);
			return true;
		}

		bool IDropTarget.ItemInteract(Point p, Point ul, KeyModifiers mods)
		{
			var mc = Geometry.ScreenToMap(p, ScreenBox);
			var gob = gstate.Scene.GetObjectAt(p, ScreenBox);
			ItemInteract.Raise(new MapClickEventArgs(0, mods, mc, p, gob));
			return true;
		}

		#endregion
	}
}
