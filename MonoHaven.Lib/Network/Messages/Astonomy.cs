namespace MonoHaven.Network.Messages
{
	public struct Astonomy
	{
		private readonly double dayTime;
		private readonly double moonPhase;

		public Astonomy(double dayTime, double moonPhase)
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
