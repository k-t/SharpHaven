using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameScene
	{
		public const int ScreenTileWidth = 46;
		public const int ScreenTileHeight = 23;

		private readonly GameState state;
		private readonly List<ObjectPart> spriteList;

		public GameScene(GameState state)
		{
			this.state = state;
			this.spriteList = new List<ObjectPart>();
		}

		public Gob GetObjectAt(Point sc)
		{
			for (int i = spriteList.Count - 1; i >= 0; i--)
				if (spriteList[i].Sprite.CheckHit(sc.X - spriteList[i].X, sc.Y - spriteList[i].Y))
					return spriteList[i].Gob;
			return null;
		}

		public void Draw(DrawingContext dc, int x, int y)
		{
			spriteList.Clear();
			foreach (var gob in state.Objects)
			{
				var sprite = gob.Sprite;
				if (sprite == null)
					continue;
				var p = WorldToScreen(gob.Position);
				spriteList.AddRange(sprite.Parts.Select(part => new ObjectPart(p, part, gob)));
			}
			spriteList.Sort(CompareParts);
			foreach (var part in spriteList)
				dc.Draw(part.Sprite, x + part.X, y + part.Y);
		}

		private int CompareParts(ObjectPart a, ObjectPart b)
		{
			if (a.Sprite.Z != b.Sprite.Z)
				return a.Sprite.Z - b.Sprite.Z;
			if (a.Y != b.Y)
				return a.Y - b.Y;
			return a.Sprite.SubZ - b.Sprite.SubZ;
		}

		public static Point ScreenToWorld(int sx, int sy)
		{
			return new Point((sx / 2 + sy) / 2, (sy - sx / 2) / 2);
		}

		public static Point ScreenToWorld(Point screen)
		{
			return ScreenToWorld(screen.X, screen.Y);
		}

		public static Point WorldToScreen(int wx, int wy)
		{
			return new Point((wx - wy) * 2, (wx + wy));
		}

		public static Point WorldToScreen(Point world)
		{
			return WorldToScreen(world.X, world.Y);
		}

		public static Point ScreenToTile(int sx, int sy)
		{
			// convert to world coordinate first
			int wx = (sx / 2 + sy) / 2;
			int wy = (sy - sx / 2) / 2;
			return new Point(wx / Map.TileWidth, wy / Map.TileHeight);
		}
		
		public static Point ScreenToTile(Point screen)
		{
			return ScreenToTile(screen.X, screen.Y);
		}

		public static Point TileToScreen(int tx, int ty)
		{
			return new Point(
				(tx - ty) * Map.TileWidth * 2,
				(tx + ty) * Map.TileHeight);
		}

		public static Point TileToScreen(Point tile)
		{
			return TileToScreen(tile.X, tile.Y);
		}

		private struct ObjectPart
		{
			public readonly int X;
			public readonly int Y;
			public readonly Gob Gob;
			public readonly SpritePart Sprite;

			public ObjectPart(Point position, SpritePart sprite, Gob gob)
			{
				X = position.X;
				Y = position.Y;
				Sprite = sprite;
				Gob = gob;
			}
		}
	}
}
