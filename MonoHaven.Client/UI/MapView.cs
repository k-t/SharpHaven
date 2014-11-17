﻿using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class MapView : Widget
	{
		private readonly GameState gstate;

		private Point worldPosition;
		private Point cameraOffset;
		private bool dragging;

		public MapView(Widget parent, GameState gstate, Point worldPosition, int playerId)
			: base(parent)
		{
			this.gstate = gstate;
			this.worldPosition = worldPosition;
			this.cameraOffset = WorldToScreen(worldPosition);
			RequestMaps();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			DrawTiles(dc);
			DrawFlavor(dc);
			DrawObjects(dc);
		}

		private void RequestMaps()
		{
			int x = worldPosition.X.Div(100 * 11);
			int y = worldPosition.Y.Div(100 * 11);

			for (int i = -5; i < 5; i++)
				for (int j = -5; j < 5; j++)
					this.gstate.Map.Request(x + i, y + j);
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

		private void DrawFlavor(DrawingContext g)
		{
			foreach (var tuple in gstate.Map.FlavorObjects)
			{
				var c = tuple.Item1;
				var t = tuple.Item2;
				var p = WorldToScreen(c);
				p = Point.Add(p, new Size(Width / 2 - cameraOffset.X, Height / 2 - cameraOffset.Y));
				if (Bounds.Contains(p))
				{
					g.Draw(t, p.X, p.Y);
				}
			}
		}

		private void DrawObjects(DrawingContext g)
		{
			foreach (var gob in gstate.Objects)
			{
				if (gob.Sprite == null)
					return;

				var p = WorldToScreen(gob.Position);
				p = Point.Add(p, new Size(Width / 2 - cameraOffset.X, Height / 2 - cameraOffset.Y));
				if (Bounds.Contains(p))
					g.Draw(gob.Sprite, p.X + gob.DrawOffset.X, p.Y + gob.DrawOffset.Y);
			}
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
			return new Point((p.X - p.Y) * 2, (p.X + p.Y));
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

		protected override void OnKeyDown(KeyEventArgs e)
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
					break;
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				Host.GrabMouse(this);
				dragging = true;
			}
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
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
