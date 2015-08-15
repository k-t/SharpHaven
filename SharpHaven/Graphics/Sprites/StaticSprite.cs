using System.Collections.Generic;

namespace SharpHaven.Graphics.Sprites
{
	public class StaticSprite : ISprite
	{
		public StaticSprite(IEnumerable<SpritePart> parts)
		{
			Parts = parts;
		}

		public IEnumerable<SpritePart> Parts { get; }

		public bool Tick(int dt)
		{
			return false;
		}
	}
}
