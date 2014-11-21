using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public class StaticSpriteFactory : SpriteFactory
	{
		public StaticSpriteFactory(Resource res) : base(res)
		{
		}

		public override ISprite Create(byte[] state)
		{
			return new StaticSprite(Parts);
		}
	}
}
