using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public interface ISprite
	{
		IEnumerable<Picture> Parts { get; }
		bool Tick(int dt);
	}
}
