using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public class ImageSprite : Sprite
	{
		public ImageSprite(Resource res) : base(res)
		{
		}

		public override void Draw(SpriteBatch batch, int x, int y)
		{
			foreach (var part in Parts.OrderBy(p => p.Z))
				DrawPart(part, batch, x, y);
		}
	}
}
