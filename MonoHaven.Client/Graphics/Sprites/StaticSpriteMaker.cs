using System.Collections;
using System.Linq;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven.Graphics.Sprites
{
	public class StaticSpriteMaker : SpriteMaker
	{
		public StaticSpriteMaker(Resource res) : base(res)
		{
		}

		public override ISprite MakeInstance(byte[] state)
		{
			var flags = state != null
				? new BitArray(state)
				: new BitArray(0);
			var parts = Parts.Where(p => p.Id < 0 || flags.IsSet(p.Id));
			return new StaticSprite(parts);
		}
	}
}
