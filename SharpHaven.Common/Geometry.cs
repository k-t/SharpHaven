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

		public static Coord2d Tilify(int mx, int my)
		{
			return new Coord2d(
				mx.Div(TileWidth) * TileWidth + TileWidth.Div(2),
				my.Div(TileHeight) * TileHeight + TileHeight.Div(2));
		}

		public static Coord2d Tilify(Coord2d mc)
		{
			return Tilify(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToMap

		public static Coord2d ScreenToMap(int sx, int sy)
		{
			return new Coord2d((sx / 2 + sy) / 2, (sy - sx / 2) / 2);
		}

		public static Coord2d ScreenToMap(Coord2d sc)
		{
			return ScreenToMap(sc.X, sc.Y);
		}

		#endregion

		#region MapToScreen

		public static Coord2d MapToScreen(int mx, int my)
		{
			return new Coord2d((mx - my) * 2, (mx + my));
		}

		public static Coord2d MapToScreen(Coord2d mc)
		{
			return MapToScreen(mc.X, mc.Y);
		}

		#endregion

		#region ScreenToTile

		public static Coord2d ScreenToTile(int sx, int sy)
		{
			// convert to world coordinate first
			int mx = (sx / 2 + sy) / 2;
			int my = (sy - sx / 2) / 2;
			return new Coord2d(mx / TileWidth, my / TileHeight);
		}

		public static Coord2d ScreenToTile(Coord2d screen)
		{
			return ScreenToTile(screen.X, screen.Y);
		}

		#endregion

		#region TileToScreen

		public static Coord2d TileToScreen(int tx, int ty)
		{
			return new Coord2d(
				(tx - ty - 1) * TileWidth * 2,
				(tx + ty) * TileHeight);
		}

		public static Coord2d TileToScreen(Coord2d tile)
		{
			return TileToScreen(tile.X, tile.Y);
		}

		#endregion

		#region MapToTile

		public static Coord2d MapToTile(int mx, int my)
		{
			return new Coord2d(mx.Div(TileWidth), my.Div(TileHeight));
		}

		public static Coord2d MapToTile(Coord2d mc)
		{
			return MapToTile(mc.X, mc.Y);
		}

		#endregion

		#region MapToGrid

		public static Coord2d MapToGrid(int mx, int my)
		{
			return new Coord2d(
				mx.Div(TileWidth).Div(GridWidth),
				my.Div(TileHeight).Div(GridHeight));
		}

		public static Coord2d MapToGrid(Coord2d mc)
		{
			return MapToGrid(mc.X, mc.Y);
		}

		#endregion

		#region TileToGrid

		public static Coord2d TileToGrid(int tx, int ty)
		{
			return new Coord2d(tx.Div(GridWidth), ty.Div(GridHeight));
		}

		public static Coord2d TileToGrid(Coord2d tc)
		{
			return TileToGrid(tc.X, tc.Y);
		}

		#endregion
	}
}
