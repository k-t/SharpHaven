using System.Collections.Generic;

namespace SharpHaven.Graphics.Sprites
{
	public interface ISprite
	{
		IEnumerable<SpritePart> Parts { get; }
		bool Tick(int dt);
	}
}
