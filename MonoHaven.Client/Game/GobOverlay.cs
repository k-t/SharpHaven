using SharpHaven.Graphics.Sprites;
using SharpHaven.Utils;

namespace SharpHaven.Game
{
	public class GobOverlay
	{
		private readonly int id;
		private readonly Delayed<ISprite> sprite;
		private readonly bool isPersistent;

		public GobOverlay(int id, Delayed<ISprite> sprite, bool isPersistent)
		{
			this.id = id;
			this.sprite = sprite;
			this.isPersistent = isPersistent;
		}

		public int Id
		{
			get { return id; }
		}

		public Delayed<ISprite> Sprite
		{
			get { return sprite; }
		}

		public bool IsPersistent
		{
			get { return isPersistent; }
		}
	}
}
