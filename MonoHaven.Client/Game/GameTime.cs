namespace MonoHaven.Game
{
	public struct GameTime
	{
		private readonly double dayTime;
		private readonly double moonPhase;

		public GameTime(double dayTime, double moonPhase)
		{
			this.dayTime = dayTime;
			this.moonPhase = moonPhase;
		}

		public bool IsNight
		{
			get { return DayTime < 0.25 || DayTime > 0.75; }
		}

		public double DayTime
		{
			get { return dayTime; }
		}

		public double MoonPhase
		{
			get { return moonPhase; }
		}
	}
}
