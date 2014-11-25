using System;
using System.Collections;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

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
			var flags = state != null
				? new BitArray(state)
				: new BitArray(0);
			var ps = Parts.Where(p => p.Id < 0 || flags.IsSet(p.Id));
			var fs = frames.Where(f => f.Id < 0 || flags.IsSet(f.Id));
			return new AnimSprite(ps, fs);
		}

		private void Init(Resource res)
		{
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
						frames[i] = new AnimFrame(anim.Id, anim.Duration);
					frames[i].Parts.AddRange(Parts.Where(x => x.Id == anim.Frames[i]));
				}
			}
			if (frames == null)
				frames = new AnimFrame[0];
		}
	}
}
