using System;
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
			var speeches = new List<Tuple<Point, GobSpeech>>();

			spriteList.Clear();
			foreach (var gob in state.Objects)
			{
				var sprite = gob.Sprite;
				if (sprite == null)
					continue;

				ISpriteEffect effect = null;
				if (gob.Health != null)
					effect = gob.Health.Effect;

				var p = Geometry.MapToScreen(gob.Position);
				spriteList.AddRange(sprite.Parts.Select(part => new ObjectPart(p, part, gob, effect)));

				if (gob.Speech != null)
					speeches.Add(Tuple.Create(p, gob.Speech));
			}

			spriteList.Sort(CompareParts);
			foreach (var part in spriteList)
			{
				if (part.Effect != null) part.Effect.Apply(dc);
				dc.Draw(part.Sprite, x + part.X, y + part.Y);
				if (part.Effect != null) part.Effect.Unapply(dc);
			}

			foreach (var speech in speeches)
				speech.Item2.Draw(dc, x + speech.Item1.X, y + speech.Item1.Y);
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
			public readonly ISpriteEffect Effect;

			public ObjectPart(Point position, Picture sprite, Gob gob, ISpriteEffect effect)
			{
				X = position.X;
				Y = position.Y;
				Sprite = sprite;
				Gob = gob;
				Effect = effect;
			}
		}
	}
}
