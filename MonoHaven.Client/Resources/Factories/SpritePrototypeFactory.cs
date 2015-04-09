using System;
using System.Collections.Generic;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Graphics.Sprites.Plants;
using NLog;

namespace MonoHaven.Resources
{
	public class SpritePrototypeFactory : IObjectFactory<SpritePrototype>
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<string, Func<Resource, SpritePrototype>> dynFacts;

		public SpritePrototypeFactory()
		{
			dynFacts = new Dictionary<string, Func<Resource, SpritePrototype>>();
			
			// known dynamic sprite factories
			dynFacts["gfx/fx/score"] = (res) => new FloatTextPrototype(res);
			dynFacts["gfx/terobjs/plants/flax"] = (res) => new GrowingPlantPrototype(res, 4, 3, 20);
			dynFacts["gfx/terobjs/plants/hemp"] = (res) => new GrowingPlantPrototype(res, 5, 3, 2);
			dynFacts["gfx/terobjs/plants/carrot"] = (res) => new GrowingPlantPrototype(res, 5, 3, 4);
			dynFacts["gfx/terobjs/plants/wheat"] = (res) => new GrowingPlantPrototype(res, 4, 2, 20, true);
			dynFacts["gfx/terobjs/plants/tobacco"] = (res) => new GrowingPlantPrototype(res, 5, 3, 2);
			dynFacts["gfx/terobjs/plants/pepper"] = (res) => new GrowingPlantPrototype(res, 4, 3, 3);
			dynFacts["gfx/terobjs/plants/hops"] = (res) => new GrowingPlantPrototype(res, 4, 3, 4);
			dynFacts["gfx/terobjs/plants/onion"] = (res) => new GrowingPlantPrototype(res, 4, 3, 4);
			dynFacts["gfx/terobjs/plants/tea"] = (res) => new GrowingPlantPrototype(res, 4, 3, 4);
			dynFacts["gfx/terobjs/plants/wine"] = (res) => new GrowingPlantPrototype(res, 4, 3, 2);
			dynFacts["gfx/terobjs/plants/pumpkin"] = (res) => new GrowingPlantPrototype(res, 7, 3, 1);
			dynFacts["gfx/terobjs/plants/pumpkin"] = (res) => new GrowingPlantPrototype(res, 7, 3, 1);
			dynFacts["gfx/terobjs/plants/beetroot"] = (res) => new GrowingPlantPrototype(res, 4, 3, 5);
			dynFacts["gfx/terobjs/plants/peas"] = (res) => new GrowingPlantPrototype(res, 5, 3, 4);
			dynFacts["gfx/terobjs/plants/poppy"] = (res) => new GrowingPlantPrototype(res, 5, 3, 10);
		}

		public SpritePrototype Create(string resName, Resource res)
		{
			// if sprite resource contains code, most likely it's a dynamic
			// factory specific for this resource
			var code = res.GetLayer<CodeEntryData>();
			if (code != null)
			{
				var factory = GetDynamicFactory(resName);
				if (factory != null)
					return factory(res);

				foreach (var codeEntry in code.Entries)
					Log.Warn("Unknown sprite class {0} in resource {1}",
						codeEntry.ClassName, resName);
			}

			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSpritePrototype(res)
				: new StaticSpritePrototype(res) as SpritePrototype;
		}

		private Func<Resource, SpritePrototype> GetDynamicFactory(string resName)
		{
			Func<Resource, SpritePrototype> fact;
			return dynFacts.TryGetValue(resName, out fact) ? fact : null;
		}
	}
}
