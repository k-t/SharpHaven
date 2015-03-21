using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapView : Widget
	{
		private readonly GameState gstate;

		private int playerId;
		private Point cameraOffset;
		private bool dragging;

		public MapView(Widget parent, GameState gstate, Point worldPoint, int playerId)
			: base(parent)
		{
			this.gstate = gstate;
			this.playerId = playerId;
			this.WorldPoint = worldPoint;
			this.cameraOffset = ToRelative(GameScene.WorldToScreen(worldPoint));
		}

		public event Action<MapClickEventArgs> MapClicked;

		public Point WorldPoint
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			RequestMaps();
			DrawTiles(dc);
			DrawScene(dc);
		}

		private void RequestMaps()
		{
			if (playerId != -1)
			{
				var player = gstate.Objects.Get(playerId);
				var ul = Map.WorldToGrid(player.Position.Add(-500));
				var br = Map.WorldToGrid(player.Position.Add(500));
				for (int y = ul.Y; y <= br.Y; y++)
					for (int x = ul.X; x <= br.X; x++)
						gstate.Map.Request(x, y);
			}
		}

		private void DrawTiles(DrawingContext g)
		{
			// get tile in the center
			var center = GameScene.ScreenToTile(cameraOffset);
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

						var p = ToRelative(GameScene.TileToScreen(tx, ty));
						p.X -= Map.TileWidth * 2;

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
			gstate.Scene.Draw(g, Width / 2 - cameraOffset.X, Height / 2 - cameraOffset.Y);
		}

		protected override void OnKeyDown(KeyEvent e)
		{
			e.Handled = true;
			switch (e.Key)
			{
				case Key.Up:
					cameraOffset.Y -= 50;
					break;
				case Key.Down:
					cameraOffset.Y += 50;
					break;
				case Key.Left:
					cameraOffset.X -= 50;
					break;
				case Key.Right:
					cameraOffset.X += 50;
					break;
				default:
					e.Handled = false;
					base.OnKeyDown(e);
					break;
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			var screenPoint = ToAbsolute(e.Position);
			var gob = gstate.Scene.GetObjectAt(screenPoint);
			if (e.Button == MouseButton.Middle)
			{
				Host.GrabMouse(this);
				dragging = true;
			}
			else
			{
				var mapPoint = GameScene.ScreenToWorld(screenPoint);
				var args = new MapClickEventArgs(e.Button, e.Modifiers, mapPoint, e.Position, gob);
				MapClicked.Raise(args);
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
				cameraOffset.X -= e.DeltaX;
				cameraOffset.Y -= e.DeltaY;
			}
		}

		/// <summary>
		/// Converts absolute screen coordinate to a relative to the current viewport.
		/// </summary>
		private Point ToRelative(Point abs)
		{
			return new Point(
				abs.X + Width / 2 - cameraOffset.X,
				abs.Y + Height / 2 - cameraOffset.Y);
		}

		/// <summary>
		/// Converts relative screen coordinate to absolute.
		/// </summary>
		private Point ToAbsolute(Point rel)
		{
			return new Point(
				rel.X - Width / 2 + cameraOffset.X,
				rel.Y - Height / 2 + cameraOffset.Y);
		}
	}
}
