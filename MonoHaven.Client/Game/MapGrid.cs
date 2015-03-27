using System.Drawing;

namespace MonoHaven.Game
{
	public class MapGrid
	{
		private readonly Point coord;
		private readonly string minimapName;
		private readonly MapTile[] tiles;

		public MapGrid(Point coord, string minimapName, MapTile[] tiles)
		{
			this.coord = coord;
			this.minimapName = minimapName;
			this.tiles = tiles;
		}

		public Point Coord
		{
			get { return coord; }
		}

		public string MinimapName
		{
			get { return minimapName; }
		}

		public MapTile this[int x, int y]
		{
			get { return tiles[x + y * Geometry.GridHeight]; }
		}
	}
}
