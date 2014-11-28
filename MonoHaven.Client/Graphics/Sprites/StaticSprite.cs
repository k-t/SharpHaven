using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class StaticSprite : ISprite
	{
		private readonly IEnumerable<Picture> parts;

		public StaticSprite(IEnumerable<Picture> parts)
		{
			this.parts = parts;
		}

		public IEnumerable<Picture> Parts
		{
			get { return parts; }
		}

		public void Tick(int dt)
		{
		}
	}
}
