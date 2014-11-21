using System;
using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public interface ISprite
	{
		IEnumerable<SpritePart> Parts { get; }
		void Tick(int dt);
	}
}
