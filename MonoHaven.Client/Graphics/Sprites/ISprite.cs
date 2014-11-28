using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public interface ISprite
	{
		IEnumerable<Picture> Parts { get; }
		void Tick(int dt);
	}
}
