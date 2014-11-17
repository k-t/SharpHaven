using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private ImageSprite sprite;

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

		public void SetSprite(ImageSprite value)
		{
			if (sprite != null) sprite.Dispose();
			sprite = value;
		}
	}
}
