using System;
using System.Collections.Generic;
using MonoHaven.Graphics.Sprites;
using NLog;

namespace MonoHaven.Resources
{
	public class SpritePrototypeFactory : IObjectFactory<SpritePrototype>
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<string, Func<Resource, SpritePrototype>> dynamicFactories;

		public SpritePrototypeFactory()
		{
			dynamicFactories = new Dictionary<string, Func<Resource, SpritePrototype>>();
			
			// known dynamic sprite factories
			dynamicFactories["gfx/fx/score"] = (res) => new FloatTextPrototype(res);
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
			return dynamicFactories.TryGetValue(resName, out fact) ? fact : null;
		}
	}
}
