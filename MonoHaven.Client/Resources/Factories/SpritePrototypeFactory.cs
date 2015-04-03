using MonoHaven.Graphics.Sprites;
using NLog;

namespace MonoHaven.Resources
{
	public class SpritePrototypeFactory : IObjectFactory<SpritePrototype>
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public SpritePrototype Create(string resName, Resource res)
		{
			var code = res.GetLayer<CodeEntryData>();
			if (code != null)
			{
				foreach (var codeEntry in code.Entries)
					Log.Warn("Unknown sprite class {0} in resource {1}",
						codeEntry.ClassName, resName);
			}

			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSpritePrototype(res)
				: new StaticSpritePrototype(res) as SpritePrototype;
		}
	}
}
