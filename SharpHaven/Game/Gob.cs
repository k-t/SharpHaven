using System.Drawing;
using System.Linq;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Utils;

namespace SharpHaven.Game
{
	public class Gob
	{
		private readonly int id;
		private readonly GobOverlayCollection overlays;
		private Delayed<ISprite> sprite;
		private Delayed<ISprite> avatar;
		private GobSpeech speech;
		private Point position;
		private GobMovement movement;
		private Point drawOffset;
		
		public Gob(int id)
		{
			this.id = id;
			this.overlays = new GobOverlayCollection();
		}

		public int Id
		{
			get { return id; }
		}

		public Point Position
		{
			get
			{
				if (movement != null)
					return movement.Position;
				if (Following != null)
					return Following.Gob.Position;
				return position;
			}
			set { position = value; }
		}

		public Point DrawOffset
		{
			get
			{
				var value = drawOffset;
				if (Following != null)
					value = value.Add(Following.Offset);
				return value;
			}
			set { drawOffset = value; }
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

		public GobOverlayCollection Overlays
		{
			get { return overlays; }
		}

		public GobFollowing Following
		{
			get;
			set;
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

			foreach (var overlay in overlays.ToList())
			{
				if (overlay.Sprite.Value != null)
				{
					var done = overlay.Sprite.Value.Tick(dt);
					if (done)
						overlays.Remove(overlay);
				}
			}
		}

		
	}
}
