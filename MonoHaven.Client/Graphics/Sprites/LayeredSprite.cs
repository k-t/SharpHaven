using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class LayeredSprite : ISprite
	{
		private readonly List<Delayed<ISprite>> sprites;

		public LayeredSprite(IEnumerable<Delayed<ISprite>> layers)
		{
			sprites = new List<Delayed<ISprite>>(layers);
		}

		public void Draw(SpriteBatch batch, int x, int y)
		{
			foreach (var sprite in sprites)
				if (sprite.Value != null)
					sprite.Value.Draw(batch, x, y);
		}

		public void Dispose()
		{
			foreach (var sprite in sprites)
				if (sprite.Value != null)
					sprite.Value.Dispose();
		}
	}
}
