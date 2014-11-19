using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int duration;
		private readonly List<SpritePart> parts;

		public AnimFrame(int duration, IEnumerable<SpritePart> parts)
		{
			this.duration = duration;
			this.parts = new List<SpritePart>(parts);
		}

		public int Duration
		{
			get { return duration; }
		}

		public List<SpritePart> Parts
		{
			get { return parts; }
		}
	}
}
