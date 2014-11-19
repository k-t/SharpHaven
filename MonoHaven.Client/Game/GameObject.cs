using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private Delayed<Sprite> sprite;
		private Delayed<Sprite> avatar;

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
			get { return sprite != null ? sprite.Value : null; }
		}

		public Drawable Avatar
		{
			get { return avatar != null ? avatar.Value : null; }
		}

		public void SetSprite(Delayed<Sprite> value)
		{
			sprite = value;
		}

		public void SetAvatar(Delayed<Sprite> value)
		{
			avatar = value;
		}
	}
}
