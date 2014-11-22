#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

		public void Tick(int dt)
		{
			foreach (var sprite in sprites)
				if (sprite.Value != null)
					sprite.Value.Tick(dt);
		}
	}
}
