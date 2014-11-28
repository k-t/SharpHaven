using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int id;
		private readonly int duration;
		private readonly List<Picture> parts;

		public AnimFrame(int id, int duration)
		{
			this.id = id;
			this.duration = duration;
			this.parts = new List<Picture>();
		}

		public int Id
		{
			get { return id; }
		}

		public int Duration
		{
			get { return duration; }
		}

		public List<Picture> Parts
		{
			get { return parts; }
		}
	}
}
