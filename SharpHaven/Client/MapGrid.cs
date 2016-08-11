using Haven;
using Haven.Utils;

namespace SharpHaven.Client
{
	public class MapGrid
	{
		private readonly Point2D coord;
		private readonly string minimapName;
		private readonly MapTile[] tiles;

		public MapGrid(Point2D coord, string minimapName, MapTile[] tiles)
		{
			this.coord = coord;
			this.minimapName = minimapName;
			this.tiles = tiles;
		}

		public Point2D Coord
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
