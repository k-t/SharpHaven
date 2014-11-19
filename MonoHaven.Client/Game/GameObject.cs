using System.Drawing;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObject
	{
		private Delayed<ISprite> sprite;
		private Delayed<ISprite> avatar;

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

		public ISprite Sprite
		{
			get { return sprite != null ? sprite.Value : null; }
		}

		public ISprite Avatar
		{
			get { return avatar != null ? avatar.Value : null; }
		}

		public void SetSprite(Delayed<ISprite> value)
		{
			sprite = value;
		}

		public void SetAvatar(Delayed<ISprite> value)
		{
			avatar = value;
		}
	}
}
