using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;

namespace SharpHaven.Client
{
	public class GameScene
	{
		public const int ScreenTileWidth = 46;
		public const int ScreenTileHeight = 23;

		private readonly ClientSession session;
		private readonly List<ObjectPart> spriteList;

		public GameScene(ClientSession session)
		{
			this.session = session;
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
			foreach (var gob in session.Objects)
			{
				var sprite = gob.Sprite;
				if (sprite == null)
					continue;

				ISpriteEffect effect = null;
				if (gob.Health != null)
					effect = gob.Health.Effect;

				int szo = 0;
				if (gob.Following != null)
					szo = gob.Following.Szo;

				var p = Geometry.MapToScreen(gob.Position);
				
				spriteList.AddRange(sprite.Parts.Select(part => new ObjectPart(p, part, gob, effect, szo)));

				foreach (var overlay in gob.Overlays)
					if (overlay.Sprite.Value != null)
						spriteList.AddRange(overlay.Sprite.Value.Parts.Select(part => new ObjectPart(p, part, gob, null, szo)));

				if (gob.Speech != null)
					speeches.Add(Tuple.Create(p, gob.Speech));
			}

			spriteList.Sort(CompareParts);
			foreach (var part in spriteList)
			{
				if (part.Effect != null) part.Effect.Apply(dc);
				var doff = part.Gob.DrawOffset;
				dc.Draw(part.Sprite, x + part.X + doff.X, y + part.Y + doff.Y);
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
			return (a.Sprite.SubZ + a.Szo) - (b.Sprite.SubZ + b.Szo);
		}

		private struct ObjectPart
		{
			public readonly int X;
			public readonly int Y;
			public readonly Gob Gob;
			public readonly SpritePart Sprite;
			public readonly ISpriteEffect Effect;
			public readonly int Szo;

			public ObjectPart(Point position, SpritePart sprite, Gob gob, ISpriteEffect effect, int szo)
			{
				X = position.X;
				Y = position.Y;
				Sprite = sprite;
				Gob = gob;
				Effect = effect;
				Szo = szo;
			}
		}
	}
}
