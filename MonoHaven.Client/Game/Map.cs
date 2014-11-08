using System;
using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Map
	{
		private readonly Tileset[] tilesets = new Tileset[256];
		private readonly TreeDictionary<Point, MapGrid> grids;
		private readonly C5Random rng;
		private readonly List<Tuple<Point, TextureRegion>> flavorObjects = new List<Tuple<Point, TextureRegion>>();

		public Map()
		{
			grids = new TreeDictionary<Point, MapGrid>(new PointComparer());
			rng = new C5Random(RandomUtils.GetSeed());
		}

		public IEnumerable<Tuple<Point, TextureRegion>> FlavorObjects
		{
			get { return flavorObjects; }
		}

		public void AddGrid(int gx, int gy, byte[] tiles)
		{
			var p = new Point(gx, gy);
			var mapTiles = new MapTile[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				var tileset = tilesets[tiles[i]];
				mapTiles[i] = new MapTile(this, GetAbsoluteTileCoord(p, i), tiles[i], tileset.GroundTiles.PickRandom(rng));
			}
			var grid = new MapGrid(p, mapTiles);
			grids[p] = grid;

			// generate flavor
			int ox = gx * Constants.GridWidth;
			int oy = gy * Constants.GridHeight;
			for (int y = 0; y < Constants.GridHeight; y++)
				for (int x = 0; x < Constants.GridWidth; x++)
				{
					var set = GetTileset(grid[x, y].Type);
					if (set.FlavorDensity != 0 && set.FlavorObjects.Count > 0)
					{
						if (rng.Next(set.FlavorDensity) == 0)
						{
							var fo = set.FlavorObjects.PickRandom(rng);
							flavorObjects.Add(Tuple.Create(new Point(
								(x + ox) * Constants.TileWidth,
								(y + oy) * Constants.TileHeight), fo));
						}
					}
				}
		}

		public MapTile GetTile(int tx, int ty)
		{
			var grid = GetGrid(tx, ty);
			if (grid == null)
				return null;

			var gtx = tx.Mod(Constants.GridWidth);
			var gty = ty.Mod(Constants.GridHeight);
			
			return grid[gtx, gty];
		}
		
		public Tileset GetTileset(byte tileType)
		{
			return tilesets[tileType];
		}

		public void SetTileset(byte tileType, Tileset tileset)
		{
			tilesets[tileType] = tileset;
		}

		private MapGrid GetGrid(int tx, int ty)
		{
			var gc = new Point(tx.Div(Constants.GridWidth), ty.Div(Constants.GridHeight));
			MapGrid grid;
			return grids.Find(ref gc, out grid) ? grid : null;
		}

		private static Point GetAbsoluteTileCoord(Point gp, int tileIndex)
		{
			return new Point(
				gp.X * Constants.GridWidth + tileIndex % Constants.GridWidth,
				gp.Y * Constants.GridHeight + tileIndex / Constants.GridHeight
			);
		}
	}
}
