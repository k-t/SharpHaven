namespace MonoHaven.Graphics.Sprites
{
	public interface ISpriteEffect
	{
		void Apply(DrawingContext dc);
		void Unapply(DrawingContext dc);
	}
}
