using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameScene
	{
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
				if (spriteList[i].Sprite.CheckHit(spriteList[i].X - sc.X, spriteList[i].Y - sc.Y))
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

		/// <summary>
		/// Converts absolute world position to absolute screen coordinate.
		/// </summary>
		private Point WorldToScreen(Point p)
		{
			return new Point((p.X - p.Y) * 2, (p.X + p.Y));
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
