using System.Collections.Generic;
using MonoHaven.Game;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Utils;

namespace MonoHaven.UI.Widgets
{
	public class Avatar
	{
		private readonly int? gobId;
		private readonly GobCache gobCache;
		private readonly ISprite sprite;

		public Avatar(IEnumerable<Delayed<ISprite>> layers)
		{
			this.sprite = layers != null ? new LayeredSprite(layers) : null;
		}

		public Avatar(int gobId, GobCache gobCache)
		{
			this.gobId = gobId;
			this.gobCache = gobCache;
		}

		public ISprite Image
		{
			get
			{
				return gobId.HasValue
					? gobCache.Get(gobId.Value).Avatar
					: sprite;
			}
		}
	}
}
