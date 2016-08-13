using Haven.Utils;
using SharpHaven.Graphics.Sprites;

namespace SharpHaven.Client
{
	public class GobOverlay
	{
		public GobOverlay(int id, Delayed<ISprite> sprite, bool isPersistent)
		{
			Id = id;
			Sprite = sprite;
			IsPersistent = isPersistent;
		}

		public GobOverlay(int id, ISprite sprite, bool isPersistent)
			: this(id, new Delayed<ISprite>(sprite), isPersistent)
		{
		}

		public GobOverlay(int id, ISprite sprite)
			: this(id, sprite, false)
		{
		}

		public GobOverlay(ISprite sprite)
			: this(-1, sprite)
		{
		}

		public int Id { get; }

		public Delayed<ISprite> Sprite { get; }

		public bool IsPersistent { get; }
	}
}
