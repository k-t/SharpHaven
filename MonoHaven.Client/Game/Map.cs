using System;
using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Messages;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Map
	{
		private readonly GameSession session;
		private readonly Tileset[] tilesets = new Tileset[256];
		private readonly TreeDictionary<Point, MapGrid> grids;
		private readonly C5Random random;
		private readonly List<Tuple<Point, ISprite>> flavorObjects = new List<Tuple<Point, ISprite>>();

		public Map(GameSession session)
		{
			this.session = session;
			grids = new TreeDictionary<Point, MapGrid>(new PointComparer());
			random = new C5Random(RandomUtils.GetSeed());
		}

		public IEnumerable<Tuple<Point, ISprite>> FlavorObjects
		{
			get { return flavorObjects; }
		}

		public MapTile GetTile(int tx, int ty)
		{
			var grid = GetGrid(Geometry.TileToGrid(tx, ty));
			if (grid == null)
				return null;

			var gtx = tx.Mod(Geometry.GridWidth);
			var gty = ty.Mod(Geometry.GridHeight);
			
			return grid[gtx, gty];
		}
		
		public Tileset GetTileset(byte tileType)
		{
			return tilesets[tileType];
		}

		public void Request(int gx, int gy)
		{
			if (!grids.Contains(new Point(gx, gy)))
			{
				grids[new Point(gx, gy)] = null; // prevent continious requests
				session.RequestData(gx, gy);
			}
		}

		public MapGrid GetGrid(int gx, int gy)
		{
			return GetGrid(new Point(gx, gy));
		}

		public MapGrid GetGrid(Point gc)
		{
			MapGrid grid;
			return grids.Find(ref gc, out grid) ? grid : null;
		}

		public void AddGrid(UpdateMapMessage message)
		{
			var gp = message.Grid;
			var tiles = new MapTile[message.Tiles.Length];
			for (int i = 0; i < message.Tiles.Length; i++)
			{
				var tile = message.Tiles[i];
				var tileset = tilesets[tile];
				if (tileset == null)
					throw new Exception(string.Format("Unknown tileset ({0})", tile));
				tiles[i] = new MapTile(this, GetAbsoluteTileCoord(gp, i), tile, tileset.GroundTiles.PickRandom(random));
			}
			var grid = new MapGrid(gp, message.MinimapName, tiles);
			grids[gp] = grid;

			// generate flavor
			int ox = gp.X * Geometry.GridWidth;
			int oy = gp.Y * Geometry.GridHeight;
			for (int y = 0; y < Geometry.GridHeight; y++)
				for (int x = 0; x < Geometry.GridWidth; x++)
				{
					var set = GetTileset(grid[x, y].Type);
					if (set.FlavorDensity != 0 && set.FlavorObjects.Count > 0)
					{
						if (random.Next(set.FlavorDensity) == 0)
						{
							var fo = set.FlavorObjects.PickRandom(random);
							flavorObjects.Add(Tuple.Create(new Point(
								(x + ox) * Geometry.TileWidth,
								(y + oy) * Geometry.TileHeight), fo));
						}
					}
				}
		}

		public void BindTileset(BindTilesetMessage message)
		{
			var tileset = App.Resources.Get<Tileset>(message.Name);
			tilesets[message.Id] = tileset;
		}

		public void Invalidate(Point gc)
		{
			grids.Remove(gc);
		}

		public void InvalidateRange(Point ul, Point br)
		{
			// TODO:
		}

		public void InvalidateAll()
		{
			grids.Clear();
			flavorObjects.Clear();
		}

		private static Point GetAbsoluteTileCoord(Point gp, int tileIndex)
		{
			return new Point(
				gp.X * Geometry.GridWidth + tileIndex % Geometry.GridWidth,
				gp.Y * Geometry.GridHeight + tileIndex / Geometry.GridHeight
			);
		}
	}
}
