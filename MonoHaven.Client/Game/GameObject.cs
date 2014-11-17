using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private Sprite sprite;

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
			get { return sprite; }
		}

		public void SetSprite(Sprite value)
		{
			if (sprite != null) sprite.Dispose();
			sprite = value;
		}
	}
}
