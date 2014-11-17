using System.Collections.Generic;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public class LayeredSprite : Sprite
	{
		private Texture tex;
		private readonly List<Sprite> sprites;

		public LayeredSprite(IEnumerable<FutureResource> layers)
		{
			sprites = new List<Sprite>(layers.Select(x => App.Instance.Resources.GetSprite(x)));
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			// TODO: find a better place to do it
			Width = sprites.Max(s => s.Width);
			Height = sprites.Max(s => s.Height);
			
			foreach (var sprite in sprites)
				sprite.Draw(batch, x, y, sprite.Width, sprite.Height);
		}
	}
}
