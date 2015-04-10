using System.Collections.Generic;

namespace SharpHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int id;
		private readonly int duration;
		private readonly List<SpritePart> parts;

		public AnimFrame(int id, int duration)
		{
			this.id = id;
			this.duration = duration;
			this.parts = new List<SpritePart>();
		}

		public int Id
		{
			get { return id; }
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
