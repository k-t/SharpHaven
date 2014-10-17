using System.Drawing;

namespace MonoHaven.Game
{
	public class MapGrid
	{
		private readonly Point coord;
		private readonly MapTile[] tiles;

		public MapGrid(Point coord, MapTile[] tiles)
		{
			this.coord = coord;
			this.tiles = tiles;
		}

		public Point Coord
		{
			get { return coord; }
		}

		public MapTile this[int x, int y]
		{
			get { return tiles[x + y * Constants.GridHeight]; }
		}
	}
}
