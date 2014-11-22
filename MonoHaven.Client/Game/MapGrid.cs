#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
			get { return tiles[x + y * Map.GridHeight]; }
		}
	}
}
