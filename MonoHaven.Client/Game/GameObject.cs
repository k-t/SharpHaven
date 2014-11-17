using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private Delayed<Sprite> sprite;

		public Point Position
		{
			get;
			set;
		}

		public Point DrawOffset
		{
			get;
			set;
		}

		public Drawable Sprite
		{
			get { return sprite.Value; }
		}

		public void SetSprite(Delayed<Sprite> value)
		{
			if (sprite != null && sprite.Value != null)
				sprite.Value.Dispose();
			sprite = value;
		}
	}
}
