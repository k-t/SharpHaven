using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class Sprite : Drawable
	{
		public static Sprite Create(FutureResource res, byte[] data)
		{
			return new ImageSprite(res, data);
		}
	}
}
