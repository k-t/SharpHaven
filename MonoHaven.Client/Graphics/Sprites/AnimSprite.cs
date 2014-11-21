using System;
using System.Collections.Generic;
using System.Linq;
namespace MonoHaven.Graphics.Sprites
{
	public class AnimSprite : ISprite
	{
		private readonly AnimFrame[] frames;
		private int frameIndex;
		private int elapsed;

		public AnimSprite(AnimFrame[] frames)
		{
			this.frames = frames;
		}

		public IEnumerable<SpritePart> Parts
		{
			get { return frames[frameIndex].Parts; }
		}

		public void Tick(int dt)
		{
			elapsed += dt;
			while (elapsed > frames[frameIndex].Duration)
			{
				elapsed -= frames[frameIndex].Duration;
				if (++frameIndex >= frames.Length)
					frameIndex = 0;
			}
		}
	}
}
