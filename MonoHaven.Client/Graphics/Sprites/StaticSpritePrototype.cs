using System.Collections;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class StaticSpritePrototype : SpritePrototype
	{
		public StaticSpritePrototype(Resource res) : base(res)
		{
		}

		public override ISprite CreateInstance(byte[] state)
		{
			var flags = state != null
				? new BitArray(state)
				: new BitArray(0);
			var parts = Parts.Where(p => p.Id < 0 || flags.IsSet(p.Id));
			return new StaticSprite(parts);
		}
	}
}
