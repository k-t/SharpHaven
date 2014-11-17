using System;
using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class LayeredSprite : Sprite
	{
		private Texture tex;
		private readonly List<Delayed<Sprite>> sprites;

		public LayeredSprite(IEnumerable<Delayed<Sprite>> layers)
		{
			sprites = new List<Delayed<Sprite>>(layers);
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			foreach (var sprite in sprites)
				if (sprite.Value != null)
				{
					// TODO: find a better place to do it
					Width = Math.Max(Width, sprite.Value.Width);
					Height = Math.Max(Height, sprite.Value.Height);
					sprite.Value.Draw(batch, x, y, sprite.Value.Width, sprite.Value.Height);
				}
		}
	}
}
