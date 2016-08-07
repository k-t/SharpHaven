using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven
{
	public static class Geometry
	{
		public const int TileWidth = 11;
		public const int TileHeight = 11;
		public const int GridWidth = 100;
		public const int GridHeight = 100;

		#region Tilify

		public static Coord2D Tilify(int mx, int my)
		{
			return new Coord2D(
				mx.Div(TileWidth) * TileWidth + TileWidth.Div(2),
				my.Div(TileHeight) * TileHeight + TileHeight.Div(2));
		}

		public static Coord2D Tilify(Coord2D mc)
		{
			return Tilify(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToMap

		public static Coord2D ScreenToMap(int sx, int sy)
		{
			return new Coord2D((sx / 2 + sy) / 2, (sy - sx / 2) / 2);
		}

		public static Coord2D ScreenToMap(Coord2D sc)
		{
			return ScreenToMap(sc.X, sc.Y);
		}

		#endregion

		#region MapToScreen

		public static Coord2D MapToScreen(int mx, int my)
		{
			return new Coord2D((mx - my) * 2, (mx + my));
		}

		public static Coord2D MapToScreen(Coord2D mc)
		{
			return MapToScreen(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToTile

		public static Coord2D ScreenToTile(int sx, int sy)
		{
			// convert to world coordinate first
			int mx = (sx / 2 + sy) / 2;
			int my = (sy - sx / 2) / 2;
			return new Coord2D(mx / TileWidth, my / TileHeight);
		}

		public static Coord2D ScreenToTile(Coord2D screen)
		{
			return ScreenToTile(screen.X, screen.Y);
		}

		#endregion

		#region TileToScreen

		public static Coord2D TileToScreen(int tx, int ty)
		{
			return new Coord2D(
				(tx - ty - 1) * TileWidth * 2,
				(tx + ty) * TileHeight);
		}

		public static Coord2D TileToScreen(Coord2D tile)
		{
			return TileToScreen(tile.X, tile.Y);
		}

		#endregion

		#region MapToTile

		public static Coord2D MapToTile(int mx, int my)
		{
			return new Coord2D(mx.Div(TileWidth), my.Div(TileHeight));
		}

		public static Coord2D MapToTile(Coord2D mc)
		{
			return MapToTile(mc.X, mc.Y);
		}

		#endregion

		#region MapToGrid

		public static Coord2D MapToGrid(int mx, int my)
		{
			return new Coord2D(
				mx.Div(TileWidth).Div(GridWidth),
				my.Div(TileHeight).Div(GridHeight));
		}

		public static Coord2D MapToGrid(Coord2D mc)
		{
			return MapToGrid(mc.X, mc.Y);
		}

		#endregion

		#region TileToGrid

		public static Coord2D TileToGrid(int tx, int ty)
		{
			return new Coord2D(tx.Div(GridWidth), ty.Div(GridHeight));
		}

		public static Coord2D TileToGrid(Coord2D tc)
		{
			return TileToGrid(tc.X, tc.Y);
		}

		#endregion
	}
}
