using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Graphics.Sprites
{
	public class LayeredSprite : ISprite
	{
		private readonly List<Delayed<ISprite>> sprites;

		public LayeredSprite(IEnumerable<Delayed<ISprite>> layers)
		{
			sprites = new List<Delayed<ISprite>>(layers);
		}

		public IEnumerable<SpritePart> Parts
		{
			get
			{
				return sprites.SelectMany(
					x => x.Value != null
						? x.Value.Parts
						: new SpritePart[0]);
			}
		}

		public void Dispose()
		{
		}
	}
}
