using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Resources
{
	public class SpritePrototypeFactory : IObjectFactory<SpritePrototype>
	{
		public SpritePrototype Create(string resName, Resource res)
		{
			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSpritePrototype(res)
				: new StaticSpritePrototype(res) as SpritePrototype;
		}
	}
}
