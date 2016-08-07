using System.Linq;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Utils;

namespace SharpHaven.Client
{
	public class Gob
	{
		private readonly GobOverlayCollection overlays;
		private Delayed<ISprite> sprite;
		private Delayed<ISprite> avatar;
		private GobSpeech speech;
		private Coord2d position;
		private GobMovement movement;
		private Coord2d drawOffset;
		
		public Gob(int id)
		{
			Id = id;
			this.overlays = new GobOverlayCollection();
		}

		public int Id { get; }

		public Coord2d Position
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

		public Coord2d DrawOffset
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
				speech?.Dispose();
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
			get { return sprite?.Value; }
		}

		public ISprite Avatar
		{
			get { return avatar?.Value; }
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

		public void StartMovement(Coord2d origin, Coord2d destination, int totalSteps)
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
			Sprite?.Tick(dt);
			movement?.Tick(dt);

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
