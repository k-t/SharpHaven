#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Collections.Generic;

namespace MonoHaven.Graphics.Sprites
{
	public class StaticSprite : ISprite
	{
		private readonly IEnumerable<SpritePart> parts;

		public StaticSprite(IEnumerable<SpritePart> parts)
		{
			this.parts = parts;
		}

		public IEnumerable<SpritePart> Parts
		{
			get { return parts; }
		}

		public void Tick(int dt)
		{
		}
	}
}
