using System;

namespace MonoHaven.Graphics.Sprites
{
	public interface ISprite : IDisposable
	{
		void Draw(SpriteBatch batch, int x, int y);
	}
}
