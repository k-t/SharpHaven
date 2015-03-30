using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GobHealth
	{
		private readonly int hp;
		private readonly ISpriteEffect effect;

		public GobHealth(int hp)
		{
			this.hp = hp;
			this.effect = new GobHealthEffect(this);
		}

		public ISpriteEffect Effect
		{
			get { return hp >= 4 ? null : effect; }
		}

		private class GobHealthEffect : ISpriteEffect
		{
			private readonly GobHealth health;

			public GobHealthEffect(GobHealth health)
			{
				this.health = health;
			}

			public void Apply(DrawingContext dc)
			{
				var c = (byte)(128 + (health.hp * 32));
				dc.SetColor(255, c, c, 255);
			}

			public void Unapply(DrawingContext dc)
			{
				dc.ResetColor();
			}
		}
	}
}
