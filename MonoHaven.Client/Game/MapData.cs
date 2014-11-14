using System.Drawing;

namespace MonoHaven.Game
{
	public class MapData
	{
		private readonly Point grid;
		private readonly byte[] tiles;

		public MapData(int gx, int gy, byte[] tiles)
		{
			this.grid = new Point(gx, gy);
			this.tiles = tiles;
		}

		public Point Grid
		{
			get { return grid; }
		}

		public byte[] Tiles
		{
			get { return tiles; }
		}
	}
}
