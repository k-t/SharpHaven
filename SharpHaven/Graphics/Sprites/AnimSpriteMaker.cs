﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Haven.Resources;
using SharpHaven.Client;
using SharpHaven.Utils;

namespace SharpHaven.Graphics.Sprites
{
	public class AnimSpriteMaker : SpriteMaker
	{
		private List<AnimLayer> anims;

		public AnimSpriteMaker(Resource res) : base(res)
		{
			anims = res.GetLayers<AnimLayer>().ToList();
		}

		public override ISprite MakeInstance(Gob owner, byte[] state)
		{
			var flags = state != null
				? new BitArray(state)
				: new BitArray(0);
			var ps = Parts.Where(p => p.Id < 0 || flags.IsSet(p.Id));
			var fs = GenerateFrames(flags);
			return new AnimSprite(ps, fs);
		}

		private IEnumerable<AnimFrame> GenerateFrames(BitArray filter)
		{
			AnimFrame[] frames = null;
			foreach (var anim in anims)
			{
				if (!(anim.Id < 0 || filter.IsSet(anim.Id)))
					continue;

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
			return frames ?? new AnimFrame[0];
		}
	}
}
