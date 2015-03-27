using System.Drawing;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public static class Geometry
	{
		public const int TileWidth = 11;
		public const int TileHeight = 11;
		public const int GridWidth = 100;
		public const int GridHeight = 100;

		#region Tilify

		public static Point Tilify(int mx, int my)
		{
			return new Point(
				mx.Div(TileWidth) * TileWidth + TileWidth.Div(2),
				my.Div(TileHeight) * TileHeight + TileHeight.Div(2));
		}

		public static Point Tilify(Point mc)
		{
			return Tilify(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToMap

		public static Point ScreenToMap(int sx, int sy)
		{
			return new Point((sx / 2 + sy) / 2, (sy - sx / 2) / 2);
		}

		public static Point ScreenToMap(Point sc)
		{
			return ScreenToMap(sc.X, sc.Y);
		}

		#endregion

		#region MapToScreen

		public static Point MapToScreen(int mx, int my)
		{
			return new Point((mx - my) * 2, (mx + my));
		}

		public static Point MapToScreen(Point mc)
		{
			return MapToScreen(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToTile

		public static Point ScreenToTile(int sx, int sy)
		{
			// convert to world coordinate first
			int mx = (sx / 2 + sy) / 2;
			int my = (sy - sx / 2) / 2;
			return new Point(mx / TileWidth, my / TileHeight);
		}

		public static Point ScreenToTile(Point screen)
		{
			return ScreenToTile(screen.X, screen.Y);
		}

		#endregion

		#region TileToScreen

		public static Point TileToScreen(int tx, int ty)
		{
			return new Point(
				(tx - ty - 1) * TileWidth * 2,
				(tx + ty) * TileHeight);
		}

		public static Point TileToScreen(Point tile)
		{
			return TileToScreen(tile.X, tile.Y);
		}

		#endregion

		#region MapToTile

		public static Point MapToTile(int mx, int my)
		{
			return new Point(mx.Div(TileWidth), my.Div(TileHeight));
		}

		public static Point MapToTile(Point mc)
		{
			return MapToTile(mc.X, mc.Y);
		}

		#endregion

		#region MapToGrid

		public static Point MapToGrid(int mx, int my)
		{
			return new Point(
				mx.Div(TileWidth).Div(GridWidth),
				my.Div(TileHeight).Div(GridHeight));
		}

		public static Point MapToGrid(Point mc)
		{
			return MapToGrid(mc.X, mc.Y);
		}

		#endregion

		#region TileToGrid

		public static Point TileToGrid(int tx, int ty)
		{
			return new Point(tx.Div(GridWidth), ty.Div(GridHeight));
		}

		public static Point TileToGrid(Point tc)
		{
			return TileToGrid(tc.X, tc.Y);
		}

		#endregion
	}
}
