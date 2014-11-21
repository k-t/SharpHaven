using System;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimSpriteFactory : SpriteFactory
	{
		private AnimFrame[] frames;

		public AnimSpriteFactory(Resource res) : base(res)
		{
			Init(res);
		}

		public override ISprite Create(byte[] state)
		{
			return new AnimSprite(frames);
		}

		private void Init(Resource res)
		{
			var staticParts = Parts.Where(p => p.Id < 0).ToList();
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
					frames[i].Parts.AddRange(Parts.Where(x => x.Id == anim.Frames[i]));
				}
			}
			if (frames == null)
				frames = new AnimFrame[0];
		}
	}
}
