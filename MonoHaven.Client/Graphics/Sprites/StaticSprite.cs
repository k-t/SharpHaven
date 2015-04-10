using System.Collections.Generic;

namespace SharpHaven.Graphics.Sprites
{
	public class StaticSprite : ISprite
	{
		private readonly IEnumerable<SpritePart> parts;

		public StaticSprite(IEnumerable<SpritePart> parts)
		{
			this.parts = parts;
		}

		public IEnumerable<SpritePart> Parts
		{
			get { return parts; }
		}

		public bool Tick(int dt)
		{
			return false;
		}
	}
}
