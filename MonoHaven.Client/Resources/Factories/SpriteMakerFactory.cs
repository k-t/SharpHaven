using System;
using System.Collections.Generic;
using NLog;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Graphics.Sprites.Fx;
using SharpHaven.Graphics.Sprites.Plants;

namespace SharpHaven.Resources
{
	public class SpriteMakerFactory : IObjectFactory<SpriteMaker>
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<string, Func<Resource, SpriteMaker>> makers;

		public SpriteMakerFactory()
		{
			makers = new Dictionary<string, Func<Resource, SpriteMaker>>();
			
			// known dynamic sprite factories
			makers["gfx/fx/score"] = (res) => new FloatTextMaker(res);
			makers["gfx/terobjs/plants/flax"] = GrowingPlantMaker.Create(4, 3, 20);
			makers["gfx/terobjs/plants/hemp"] = GrowingPlantMaker.Create(5, 3, 2);
			makers["gfx/terobjs/plants/carrot"] = GrowingPlantMaker.Create(5, 3, 4);
			makers["gfx/terobjs/plants/wheat"] = GrowingPlantMaker.Create(4, 2, 20, true);
			makers["gfx/terobjs/plants/tobacco"] = GrowingPlantMaker.Create(5, 3, 2);
			makers["gfx/terobjs/plants/pepper"] = GrowingPlantMaker.Create(4, 3, 3);
			makers["gfx/terobjs/plants/hops"] = GrowingPlantMaker.Create(4, 3, 4);
			makers["gfx/terobjs/plants/onion"] = GrowingPlantMaker.Create(4, 3, 4);
			makers["gfx/terobjs/plants/tea"] = GrowingPlantMaker.Create(4, 3, 4);
			makers["gfx/terobjs/plants/wine"] = GrowingPlantMaker.Create(4, 3, 2);
			makers["gfx/terobjs/plants/pumpkin"] = GrowingPlantMaker.Create(7, 3, 1);
			makers["gfx/terobjs/plants/pumpkin"] = GrowingPlantMaker.Create(7, 3, 1);
			makers["gfx/terobjs/plants/beetroot"] = GrowingPlantMaker.Create(4, 3, 5);
			makers["gfx/terobjs/plants/peas"] = GrowingPlantMaker.Create(5, 3, 4);
			makers["gfx/terobjs/plants/poppy"] = GrowingPlantMaker.Create(5, 3, 10);
		}

		public SpriteMaker Create(string resName, Resource res)
		{
			// if sprite resource contains code, most likely it's a dynamic
			// factory specific for this resource
			var code = res.GetLayer<CodeEntryData>();
			if (code != null)
			{
				var maker = FindMaker(resName);
				if (maker != null)
					return maker(res);

				foreach (var codeEntry in code.Entries)
					Log.Warn("Unknown sprite class {0} in resource {1}",
						codeEntry.ClassName, resName);
			}

			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSpriteMaker(res)
				: new StaticSpriteMaker(res) as SpriteMaker;
		}

		private Func<Resource, SpriteMaker> FindMaker(string resName)
		{
			Func<Resource, SpriteMaker> fact;
			return makers.TryGetValue(resName, out fact) ? fact : null;
		}
	}
}
