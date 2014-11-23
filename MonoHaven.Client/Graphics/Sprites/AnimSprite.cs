using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimSprite : ISprite
	{
		private readonly IEnumerable<SpritePart> parts;
		private readonly AnimFrame[] frames;
		private int frameIndex;
		private int elapsed;

		public AnimSprite(IEnumerable<SpritePart> parts, IEnumerable<AnimFrame> frames)
		{
			this.parts = parts;
			this.frames = frames.ToArray();
			if (this.frames.Length == 0)
			{
				this.frames = new AnimFrame[1];
				this.frames[0] = new AnimFrame(-1, 10000);
			}
		}

		public IEnumerable<SpritePart> Parts
		{
			get { return parts.Concat(frames[frameIndex].Parts); }
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
