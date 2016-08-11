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

		public int Id { get; }

		public Delayed<ISprite> Sprite { get; }

		public bool IsPersistent { get; }
	}
}
