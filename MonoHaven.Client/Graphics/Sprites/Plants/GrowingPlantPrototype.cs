using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites.Plants
{
	public class GrowingPlantPrototype : SpritePrototype
	{
		private readonly int num;
		private readonly Picture[,] strands;
		private readonly NegData neg;

		public GrowingPlantPrototype(
			Resource res,
			int stages,
			int variants,
			int num,
			bool rev = false)
			: base(res)
		{
			this.num = num;
			this.neg = res.GetLayer<NegData>();
			this.strands = new Picture[stages, variants];

			foreach (var part in Parts.Where(x => x.Id != -1))
			{
				int stage = rev ? part.Id / variants : part.Id % stages;
				int variant = rev ? part.Id % variants : part.Id / stages;
				this.strands[stage, variant] = part;
			}
		}

		public override ISprite CreateInstance(byte[] state)
		{
			var rnd = new Random(RandomUtils.GetSeed()); // TODO: gobId should be used as seed
			var stage = state[0];
			var parts = new List<Picture>();
			parts.AddRange(Parts.Where(x => x.Id == -1));

			for (int i = 0; i < num; i++)
			{
				var c = new Point(
					rnd.Next(neg.Hitbox.Width),
					rnd.Next(neg.Hitbox.Height))
					.Add(neg.Hitbox.Location);
				var s = strands[stage, rnd.Next(strands.GetLength(1))];
				parts.Add(new Picture(0, s.Tex,
					Geometry.MapToScreen(c).Sub(s.Width / 2, s.Height),
					s.Z,
					s.SubZ, s.Hitmask));
			}

			return new StaticSprite(parts);
		}
	}
}
