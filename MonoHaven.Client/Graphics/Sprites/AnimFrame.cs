namespace MonoHaven.Graphics.Sprites
{
	public class AnimFrame
	{
		private readonly int duration;
		private readonly TextureSlice tex;

		public AnimFrame(int duration, TextureSlice tex)
		{
			this.duration = duration;
			this.tex = tex;
		}

		public int Duration
		{
			get { return duration; }
		}

		public TextureSlice Tex
		{
			get { return tex; }
		}
	}
}
