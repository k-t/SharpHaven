using System;
using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public interface ISprite : IDisposable
	{
		IEnumerable<SpritePart> Parts { get; }
	}
}
