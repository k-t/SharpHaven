using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;

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
				var p = Geometry.MapToScreen(gob.Position);
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

		private struct ObjectPart
		{
			public readonly int X;
			public readonly int Y;
			public readonly Gob Gob;
			public readonly Picture Sprite;

			public ObjectPart(Point position, Picture sprite, Gob gob)
			{
				X = position.X;
				Y = position.Y;
				Sprite = sprite;
				Gob = gob;
			}
		}
	}
}
