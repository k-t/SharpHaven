using System;
using System.Collections.Generic;
using C5;
using Haven;
using Haven.Messaging.Messages;
using Haven.Utils;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Utils;

namespace SharpHaven.Client
{
	public class Map
	{
		private readonly Tileset[] tilesets = new Tileset[256];
		private readonly TreeDictionary<Point2D, MapGrid> grids;
		private readonly List<Tuple<Point2D, ISprite>> flavorObjects = new List<Tuple<Point2D, ISprite>>();
		private readonly List<MapOverlay> overlays;

		public Map()
		{
			grids = new TreeDictionary<Point2D, MapGrid>(new Coord2DComparer());
			overlays = new List<MapOverlay>();
		}

		public System.Collections.Generic.ICollection<MapOverlay> Overlays
		{
			get { return overlays; }
		}

		public IEnumerable<Tuple<Point2D, ISprite>> FlavorObjects
		{
			get { return flavorObjects; }
		}
		
		public MapGrid GetGrid(int gx, int gy)
		{
			return GetGrid(new Point2D(gx, gy));
		}

		public MapGrid GetGrid(Point2D gc)
		{
			MapGrid grid;
			return grids.Find(ref gc, out grid) ? grid : null;
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

		public void AddGrid(MapUpdateGrid message)
		{
			var random = new C5Random(RandomUtils.GetSeed(message.Coord));
			var gc = message.Coord;
			var tiles = new MapTile[message.Tiles.Length];
			for (int i = 0; i < message.Tiles.Length; i++)
			{
				var tile = message.Tiles[i];
				var overlay = message.Overlays[i];
				var tileset = tilesets[tile];
				if (tileset == null)
					throw new Exception($"Unknown tileset ({tile})");
				tiles[i] = new MapTile(this, GetAbsoluteTileCoord(gc, i), tile, overlay, tileset.GroundTiles.PickRandom(random));
			}
			var grid = new MapGrid(gc, message.MinimapName, tiles);
			grids[gc] = grid;

			// generate flavor
			int ox = gc.X * Geometry.GridWidth;
			int oy = gc.Y * Geometry.GridHeight;
			for (int y = 0; y < Geometry.GridHeight; y++)
				for (int x = 0; x < Geometry.GridWidth; x++)
				{
					var set = GetTileset(grid[x, y].Type);
					if (set.FlavorDensity != 0 && set.FlavorObjects.Count > 0)
					{
						if (random.Next(set.FlavorDensity) == 0)
						{
							var fo = set.FlavorObjects.PickRandom(random);
							flavorObjects.Add(Tuple.Create(new Point2D(
								(x + ox) * Geometry.TileWidth,
								(y + oy) * Geometry.TileHeight), fo));
						}
					}
				}
		}

		public void BindTileset(TilesetBinding binding)
		{
			var tileset = App.Resources.Get<Tileset>(binding.Name);
			tilesets[binding.Id] = tileset;
		}

		public void InvalidateRegion(Rect region)
		{
			// TODO:
		}

		public void InvalidateAll()
		{
			grids.Clear();
			flavorObjects.Clear();
		}

		private static Point2D GetAbsoluteTileCoord(Point2D gc, int tileIndex)
		{
			return new Point2D(
				gc.X * Geometry.GridWidth + tileIndex % Geometry.GridWidth,
				gc.Y * Geometry.GridHeight + tileIndex / Geometry.GridHeight
			);
		}
	}
}
