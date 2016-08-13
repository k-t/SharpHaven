using System;
using System.Collections.Generic;
using System.Linq;
using Haven.Resources;
using Haven.Utils;
using SharpHaven.Client;
using SharpHaven.Utils;

namespace SharpHaven.Graphics.Sprites.Plants
{
	public class GaussianPlantMaker : SpriteMaker
	{
		private readonly int num;
		private readonly SpritePart[] strands;
		private readonly NegLayer neg;

		private GaussianPlantMaker(Resource res, int num)
			: base(res)
		{
			this.neg = res.GetLayer<NegLayer>();
			this.num = num;
			this.strands = Parts.Where(x => x.Id != -1).ToArray();
		}

		public static Func<Resource, SpriteMaker> Create(int num)
		{
			return res => new GaussianPlantMaker(res, num);
		}

		public override ISprite MakeInstance(Gob owner, byte[] state)
		{
			var rnd = new Random(owner?.Id ?? RandomUtils.GetSeed());
			var parts = new List<SpritePart>();
			parts.AddRange(Parts.Where(x => x.Id == -1));
			for (int i = 0; i < num; i++)
			{
				var s = strands[rnd.Next(strands.Length)];
				var cx = (int)(rnd.NextGaussian() * neg.Hitbox.Width / 2);
				var cy = (int)(rnd.NextGaussian() * neg.Hitbox.Height / 2);
				var offset = Geometry.MapToScreen(cx, cy).Sub(s.Width / 2, s.Height);
				parts.Add(new SpritePart(s.Image, offset, s.Z, s.SubZ));
			}
			return new StaticSprite(parts);
		}
	}
}
