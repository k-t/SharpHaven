using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class MapView : Widget
	{
		private readonly GameState gstate;

		private int playerId;
		private Point cameraOffset;
		private bool dragging;

		public MapView(Widget parent, GameState gstate, Point worldCoord, int playerId)
			: base(parent)
		{
			this.gstate = gstate;
			this.playerId = playerId;
			this.WorldCoord = worldCoord;
			this.cameraOffset = WorldToScreen(worldCoord);
		}

		public event Action<MapClickEventArgs> MapClicked;

		public Point WorldCoord
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

				int x = player.Position.X.Div(100 * 11);
				int y = player.Position.Y.Div(100 * 11);

				for (int i = -5; i < 5; i++)
					for (int j = -5; j < 5; j++)
						gstate.Map.Request(x + i, y + j);
			}
		}

		private void DrawTiles(DrawingContext g)
		{
			// get tile in the center
			var center = ScreenToTile(cameraOffset);
			// how much tiles fit onto screen vertically and horizontally
			var h = Width / (Constants.ScreenTileWidth * 2);
			var v = Height / (Constants.ScreenTileHeight * 2);
			// draw all tiles around the center one
			for (int x = -(h + 4); x <= h + 2; x++)
				for (int y = -(v + 3); y <= v + 2; y++)
					for (int i = 0; i < 2; i++)
					{
						int tx = center.X + x + y;
						int ty = center.Y + y - x - i;

						var p = TileToScreen(new Point(tx, ty));
						p = Point.Add(p, new Size(Width / 2 - cameraOffset.X, Height / 2 - cameraOffset.Y));
						p.X -= Constants.TileWidth * 2;

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

		/// <summary>
		/// Converts absolute tile position to absolute screen coordinate.
		/// </summary>
		private Point TileToScreen(Point p)
		{
			return new Point(
				(p.X - p.Y) * Constants.TileWidth * 2,
				(p.X + p.Y) * Constants.TileHeight);
		}

		/// <summary>
		/// Converts absolute world position to absolute screen coordinate.
		/// </summary>
		private Point WorldToScreen(Point p)
		{
			return new Point(
				(p.X - p.Y) * 2 + Width / 2 - cameraOffset.X,
				(p.X + p.Y) + Height / 2 - cameraOffset.Y);
		}

		/// <summary>
		/// Converts absolute screen position to absolute tile position.
		/// </summary>
		private Point ScreenToTile(Point p)
		{
			return new Point(
				(p.X / (Constants.TileWidth * 2) + p.Y / Constants.TileHeight) / 2,
				(p.Y / Constants.TileHeight - p.X / (Constants.TileWidth * 2)) / 2);
		}

		/// <summary>
		/// Converts absolute screen coordinate to absolute world position.
		/// </summary>
		private Point ScreenToWorld(Point p)
		{
			p = new Point(
				p.X - Width / 2 + cameraOffset.X,
				p.Y - Height / 2 + cameraOffset.Y);
			return new Point((p.X / 2 + p.Y) / 2, (p.Y - p.X / 2) / 2);
		}

		protected override bool OnKeyDown(KeyboardKeyEventArgs e)
		{
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
					return base.OnKeyDown(e);
			}
			return true;
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			var gob = gstate.Scene.GetObjectAt(new Point(
				e.Position.X - Width / 2 + cameraOffset.X,
				e.Position.Y - Height / 2 + cameraOffset.Y));
			if (e.Button == MouseButton.Middle)
			{
				Host.GrabMouse(this);
				dragging = true;
			}
			else
			{
				var mapPoint = ScreenToWorld(e.Position);
				var args = new MapClickEventArgs(e.Button, mapPoint, e.Position, gob);
				MapClicked.Raise(args);
			}
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Middle)
			{
				Host.ReleaseMouse();
				dragging = false;
			}
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (dragging)
			{
				cameraOffset.X -= e.XDelta;
				cameraOffset.Y -= e.YDelta;
			}
		}
	}
}
