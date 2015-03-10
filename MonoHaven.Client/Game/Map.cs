using System;
using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Network.Messages;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Map
	{
		public const int TileWidth = 11;
		public const int TileHeight = 11;
		public const int GridWidth = 100;
		public const int GridHeight = 100;

		private readonly GameSession session;
		private readonly Tileset[] tilesets = new Tileset[256];
		private readonly TreeDictionary<Point, MapGrid> grids;
		private readonly C5Random random;
		private readonly List<Tuple<Point, ISprite>> flavorObjects = new List<Tuple<Point, ISprite>>();

		public Map(GameSession session)
		{
			grids = new TreeDictionary<Point, MapGrid>(new PointComparer());
			random = new C5Random(RandomUtils.GetSeed());

			this.session = session;
			this.session.TilesetBound += BindTileset;
			this.session.MapDataAvailable += AddGrid;
		}

		public IEnumerable<Tuple<Point, ISprite>> FlavorObjects
		{
			get { return flavorObjects; }
		}

		public MapTile GetTile(int tx, int ty)
		{
			var grid = GetGrid(tx, ty);
			if (grid == null)
				return null;

			var gtx = tx.Mod(GridWidth);
			var gty = ty.Mod(GridHeight);
			
			return grid[gtx, gty];
		}
		
		public Tileset GetTileset(byte tileType)
		{
			return tilesets[tileType];
		}

		public void Request(int gx, int gy)
		{
			if (!grids.Contains(new Point(gx, gy)))
				session.RequestData(gx, gy);
		}

		private MapGrid GetGrid(int tx, int ty)
		{
			var gc = new Point(tx.Div(GridWidth), ty.Div(GridHeight));
			MapGrid grid;
			return grids.Find(ref gc, out grid) ? grid : null;
		}

		private void AddGrid(MapData data)
		{
			var gp = data.Grid;
			var tiles = new MapTile[data.Tiles.Length];
			for (int i = 0; i < data.Tiles.Length; i++)
			{
				var tile = data.Tiles[i];
				var tileset = tilesets[tile];
				if (tileset == null)
					throw new Exception(string.Format("Unknown tileset ({0})", tile));
				tiles[i] = new MapTile(this, GetAbsoluteTileCoord(gp, i), tile, tileset.GroundTiles.PickRandom(random));
			}
			var grid = new MapGrid(gp, tiles);
			grids[gp] = grid;

			// generate flavor
			int ox = gp.X * GridWidth;
			int oy = gp.Y * GridHeight;
			for (int y = 0; y < GridHeight; y++)
				for (int x = 0; x < GridWidth; x++)
				{
					var set = GetTileset(grid[x, y].Type);
					if (set.FlavorDensity != 0 && set.FlavorObjects.Count > 0)
					{
						if (random.Next(set.FlavorDensity) == 0)
						{
							var fo = set.FlavorObjects.PickRandom(random);
							flavorObjects.Add(Tuple.Create(new Point(
								(x + ox) * TileWidth,
								(y + oy) * TileHeight), fo));
						}
					}
				}
		}

		private void BindTileset(TilesetBinding binding)
		{
			var tileset = App.Resources.GetTileset(binding.Name);
			tilesets[binding.Id] = tileset;
		}

		private static Point GetAbsoluteTileCoord(Point gp, int tileIndex)
		{
			return new Point(
				gp.X * GridWidth + tileIndex % GridWidth,
				gp.Y * GridHeight + tileIndex / GridHeight
			);
		}
	}
}
