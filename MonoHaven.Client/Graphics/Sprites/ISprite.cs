#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
