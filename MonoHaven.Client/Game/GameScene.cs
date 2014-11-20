using System.Drawing;
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
			foreach (var sceneObject in state.Objects)
			{
				var sprite = sceneObject.Sprite;
				if (sprite == null)
					continue;
				var p = WorldToScreen(sceneObject.Position);
				dc.Draw(sprite, p.X + x, p.Y + y);
			}
		}

		/// <summary>
		/// Converts absolute world position to absolute screen coordinate.
		/// </summary>
		private Point WorldToScreen(Point p)
		{
			return new Point((p.X - p.Y) * 2, (p.X + p.Y));
		}
	}
}
