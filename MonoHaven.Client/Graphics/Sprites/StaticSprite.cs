using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
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

		public void Tick(int dt)
		{
		}
	}
}
