using System;
using System.Collections.Generic;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimSprite : Sprite
	{
		private AnimFrame[] frames;
		private int frameIndex;

		public AnimSprite(Resource res) : base(res)
		{
			Init(res);
		}

		public override IEnumerable<SpritePart> Parts
		{
			get { Tick(); return frames[frameIndex].Parts; }
		}

		private void Tick()
		{
			frameIndex++;
			if (frameIndex >= frames.Length)
				frameIndex = 0;
		}

		private void Init(Resource res)
		{
			var staticParts = parts.Where(p => p.Id < 0).ToList();
			var anims = res.GetLayers<AnimData>();
			foreach (var anim in anims)
			{
				if (frames == null)
					frames = new AnimFrame[anim.Frames.Length];
				if (frames.Length != anim.Frames.Length)
					throw new Exception("Attempting to combine animations of different lengths");
				for (int i = 0; i < frames.Length; i++)
				{
					if (frames[i] == null)
						frames[i] = new AnimFrame(anim.Duration, staticParts);
					frames[i].Parts.AddRange(parts.Where(x => x.Id == anim.Frames[i]));
				}
			}
			if (frames == null)
				frames = new AnimFrame[0];
			foreach (var frame in frames)
				frame.Parts.Sort((a, b) => a.Z - b.Z);
		}
	}
}
