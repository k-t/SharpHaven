﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites.Plants
{
	public class GrowingPlantMaker : SpriteMaker
	{
		private readonly int num;
		private readonly SpritePart[,] strands;
		private readonly NegData neg;

		private GrowingPlantMaker(Resource res, int stages, int variants, int num, bool rev)
			: base(res)
		{
			this.num = num;
			this.neg = res.GetLayer<NegData>();
			this.strands = new SpritePart[stages, variants];

			foreach (var part in Parts.Where(x => x.Id != -1))
			{
				int stage = rev ? part.Id / variants : part.Id % stages;
				int variant = rev ? part.Id % variants : part.Id / stages;
				strands[stage, variant] = part;
			}
		}

		public static Func<Resource, SpriteMaker> Create(int stages, int variants, int num, bool rev = false)
		{
			return res => new GrowingPlantMaker(res, stages, variants, num, rev);
		}

		public override ISprite MakeInstance(byte[] state)
		{
			var rnd = new Random(RandomUtils.GetSeed()); // TODO: gobId should be used as seed
			var stage = state[0];
			var parts = new List<SpritePart>();

			parts.AddRange(Parts.Where(x => x.Id == -1));

			for (int i = 0; i < num; i++)
			{
				var c = new Point(
					rnd.Next(neg.Hitbox.Width),
					rnd.Next(neg.Hitbox.Height))
					.Add(neg.Hitbox.Location);
				var s = strands[stage, rnd.Next(strands.GetLength(1))];
				parts.Add(new SpritePart(s.Image,
					Geometry.MapToScreen(c).Sub(s.Width / 2, s.Height),
					s.Z,
					s.SubZ));
			}

			return new StaticSprite(parts);
		}
	}
}
