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

		public Map()
		{
			grids = new TreeDictionary<Point, MapGrid>(new PointComparer());
			rng = new C5Random(RandomUtils.GetSeed());

			// load all existing tilesets
			tilesets[255] = ResourceManager.LoadTileset("gfx/tiles/nil/nil");
			tilesets[26] = ResourceManager.LoadTileset("gfx/tiles/mountain/mountain");
			tilesets[25] = ResourceManager.LoadTileset("gfx/tiles/mountain/mountain");
			tilesets[24] = ResourceManager.LoadTileset("gfx/tiles/floor-mine/mine");
			tilesets[23] = ResourceManager.LoadTileset("gfx/tiles/mountain/mountain");
			tilesets[22] = ResourceManager.LoadTileset("gfx/tiles/floor-mine/mine");
			tilesets[21] = ResourceManager.LoadTileset("gfx/tiles/floor-wood/floor-wood");
			tilesets[20] = ResourceManager.LoadTileset("gfx/tiles/playa/playa");
			tilesets[19] = ResourceManager.LoadTileset("gfx/tiles/dirt/dirt");
			tilesets[18] = ResourceManager.LoadTileset("gfx/tiles/fen/fen");
			tilesets[17] = ResourceManager.LoadTileset("gfx/tiles/bog/bog");
			tilesets[16] = ResourceManager.LoadTileset("gfx/tiles/swamp/swamp");
			tilesets[15] = ResourceManager.LoadTileset("gfx/tiles/moor/moor");
			tilesets[14] = ResourceManager.LoadTileset("gfx/tiles/heath/heath");
			tilesets[13] = ResourceManager.LoadTileset("gfx/tiles/grass/grass");
			tilesets[12] = ResourceManager.LoadTileset("gfx/tiles/dwald/wald");
			tilesets[11] = ResourceManager.LoadTileset("gfx/tiles/wald/wald");
			tilesets[10] = ResourceManager.LoadTileset("gfx/tiles/wald/wald");
			tilesets[9] = ResourceManager.LoadTileset("gfx/tiles/plowed/plowed");
			tilesets[8] = ResourceManager.LoadTileset("gfx/tiles/floor-stone/stone");
			tilesets[7] = ResourceManager.LoadTileset("gfx/tiles/brick/white");
			tilesets[6] = ResourceManager.LoadTileset("gfx/tiles/brick/blue");
			tilesets[5] = ResourceManager.LoadTileset("gfx/tiles/brick/black");
			tilesets[4] = ResourceManager.LoadTileset("gfx/tiles/brick/yellow");
			tilesets[3] = ResourceManager.LoadTileset("gfx/tiles/brick/red");
			tilesets[2] = ResourceManager.LoadTileset("gfx/tiles/gold/gold");
			tilesets[1] = ResourceManager.LoadTileset("gfx/tiles/water/water");
			tilesets[0] = ResourceManager.LoadTileset("gfx/tiles/water/deep");
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
			grids[p] = new MapGrid(p, mapTiles);
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

		public Tileset GetTileset(int tileType)
		{
			return tilesets[tileType];
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
