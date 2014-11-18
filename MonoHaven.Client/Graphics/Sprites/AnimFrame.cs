namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int duration;
		private readonly TextureRegion tex;

		public AnimFrame(int duration, TextureRegion tex)
		{
			this.duration = duration;
			this.tex = tex;
		}

		public int Duration
		{
			get { return duration; }
		}

		public TextureRegion Tex
		{
			get { return tex; }
		}
	}
}
