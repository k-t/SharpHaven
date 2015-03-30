using System.Drawing;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Gob
	{
		private readonly int id;
		private Delayed<ISprite> sprite;
		private Delayed<ISprite> avatar;
		private GobSpeech speech;
		private Point position;
		private GobMovement movement;
		
		public Gob(int id)
		{
			this.id = id;
		}

		public int Id
		{
			get { return id; }
		}

		public Point Position
		{
			get { return (movement != null) ? movement.Position : position; }
			set { position = value; }
		}

		public Point DrawOffset
		{
			get;
			set;
		}

		public KinInfo KinInfo
		{
			get;
			set;
		}

		public GobSpeech Speech
		{
			get { return speech; }
			set
			{
				if (speech != null)
					speech.Dispose();
				speech = value;
			}
		}

		public GobHealth Health
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

		public void StartMovement(Point origin, Point destination, int totalSteps)
		{
			movement = new GobMovement(origin, destination, totalSteps);
		}

		public void AdjustMovement(int step)
		{
			if (movement == null)
				return;

			if (step < 0 || step >= movement.TotalSteps)
				movement = null;
			else
				movement.Adjust(step);
		}

		public void Tick(int dt)
		{
			if (Sprite != null)
				Sprite.Tick(dt);

			if (movement != null)
				movement.Tick(dt);
		}

		
	}
}
