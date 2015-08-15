using System.Collections.Generic;

namespace SharpHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		public AnimFrame(int id, int duration)
		{
			Id = id;
			Duration = duration;
			Parts = new List<SpritePart>();
		}

		public int Id { get; }

		public int Duration { get; }

		public List<SpritePart> Parts { get; }
	}
}
