using System.Collections.Generic;
using SharpHaven.Game;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
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
