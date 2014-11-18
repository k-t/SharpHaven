namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int duration;
		private readonly Texture tex;

		public AnimFrame(int duration, Texture tex)
		{
			this.duration = duration;
			this.tex = tex;
		}

		public int Duration
		{
			get { return duration; }
		}

		public Texture Tex
		{
			get { return tex; }
		}
	}
}
