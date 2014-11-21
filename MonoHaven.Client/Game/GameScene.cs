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

		public GameScene(GameState state)
		{
			this.state = state;
		}

		public void Draw(DrawingContext dc, int x, int y)
		{
			var drawList = new List<ObjectPart>();

			foreach (var sceneObject in state.Objects)
			{
				var sprite = sceneObject.Sprite;
				if (sprite == null)
					continue;
				var p = WorldToScreen(sceneObject.Position);
				drawList.AddRange(sprite.Parts.Select(part => new ObjectPart(p, part)));
			}

			drawList.Sort(CompareParts);

			foreach (var part in drawList)
				dc.Draw(part.Sprite, x + part.Position.X, y + part.Position.Y);
		}

		private int CompareParts(ObjectPart a, ObjectPart b)
		{
			if (a.Sprite.Z != b.Sprite.Z)
				return a.Sprite.Z - b.Sprite.Z;
			if (a.Position.Y != b.Position.Y)
				return a.Position.Y - b.Position.Y;
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
			public readonly Point Position;
			public readonly SpritePart Sprite;

			public ObjectPart(Point position, SpritePart sprite)
			{
				Position = position;
				Sprite = sprite;
			}
		}
	}
}
