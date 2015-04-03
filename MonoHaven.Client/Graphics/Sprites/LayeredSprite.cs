using System.Collections.Generic;
using System.Linq;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class LayeredSprite : ISprite
	{
		private readonly List<Delayed<ISprite>> sprites;

		public LayeredSprite(IEnumerable<Delayed<ISprite>> layers)
		{
			sprites = new List<Delayed<ISprite>>(layers);
		}

		public IEnumerable<Picture> Parts
		{
			get
			{
				return sprites.SelectMany(
					x => x.Value != null
						? x.Value.Parts
						: new Picture[0]);
			}
		}

		public bool Tick(int dt)
		{
			foreach (var sprite in sprites)
				if (sprite.Value != null)
					sprite.Value.Tick(dt);
			return false;
		}
	}
}
