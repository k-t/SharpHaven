#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Collections;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class StaticSpriteFactory : SpriteFactory
	{
		public StaticSpriteFactory(Resource res) : base(res)
		{
		}

		public override ISprite Create(byte[] state)
		{
			var flags = state != null
				? new BitArray(state)
				: new BitArray(0);
			var parts = Parts.Where(p => p.Id < 0 || flags.IsSet(p.Id));
			return new StaticSprite(parts);
		}
	}
}
