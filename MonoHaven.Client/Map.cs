using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C5;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven
{
	public class Map
	{
		private readonly Tileset[] tilesets = new Tileset[256];
		private readonly TreeDictionary<Point, MapGrid> grids;

		public Map()
		{
			grids = new TreeDictionary<Point, MapGrid>(new PointComparer());

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

		public void AddGrid(int x, int y, byte[] tiles)
		{
			var p = new Point(x, y);
			var mapTiles = new MapTile[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				var tileset = tilesets[tiles[i]];
				mapTiles[i] = new MapTile(tiles[i], tileset.GroundTiles.PickRandom());
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

		public void GenerateTransitions(int tx, int ty, MapTile tile)
		{
			if (tile.Transitions != null)
				return;

			var tr = new int[3, 3];
			for(int y = -1; y <= 1; y++)
			{
				for(int x = -1; x <= 1; x++)
				{
					if((x == 0) && (y == 0))
						continue;
					var t = GetTile(tx + x, ty + y);
					if (t == null)
					{ 
						tile.Transitions = new Texture[0];
						return;
					}
					tr[x + 1, y + 1] = t.Type;
				}
			}
			if(tr[0,0] >= tr[1,0]) tr[0,0] = -1;
			if(tr[0,0] >= tr[0,1]) tr[0,0] = -1;
			if(tr[2,0] >= tr[1,0]) tr[2,0] = -1;
			if(tr[2,0] >= tr[2,1]) tr[2,0] = -1;
			if(tr[0,2] >= tr[0,1]) tr[0,2] = -1;
			if(tr[0,2] >= tr[1,2]) tr[0,2] = -1;
			if(tr[2,2] >= tr[2,1]) tr[2,2] = -1;
			if(tr[2,2] >= tr[1,2]) tr[2,2] = -1;
			int[] bx = {0, 1, 2, 1};
			int[] by = {1, 0, 1, 2};
			int[] cx = {0, 2, 2, 0};
			int[] cy = {0, 0, 2, 2};
			var buf = new List<Texture>();
			for(int i = tile.Type - 1; i >= 0; i--)
			{
				var set = tilesets[i];
				if (set == null || set.BorderTransitions == null || set.CrossTransitions == null)
					continue;
				int bm = 0, cm = 0;
				for(int o = 0; o < 4; o++) {
					if(tr[bx[o],by[o]] == i)
					bm |= 1 << o;
					if(tr[cx[o],cy[o]] == i)
					cm |= 1 << o;
				}
				if(bm != 0)
					buf.Add(set.BorderTransitions[bm - 1].PickRandom());
				if(cm != 0)
					buf.Add(set.CrossTransitions[cm - 1].PickRandom());
			}
			tile.Transitions = buf.ToArray();
		}

		private MapGrid GetGrid(int tx, int ty)
		{
			var gc = new Point(tx.Div(Constants.GridWidth), ty.Div(Constants.GridHeight));
			MapGrid grid;
			return grids.Find(ref gc, out grid) ? grid : null;
		}
		
	}
}
