namespace SharpHaven.Client
{
	public class GameTime
	{
		public GameTime(double dayTime, double moonPhase)
		{
			DayTime = dayTime;
			MoonPhase = moonPhase;
		}

		public double DayTime { get; }

		public double MoonPhase { get; }

		public bool IsNight
		{
			get { return DayTime < 0.25 || DayTime > 0.75; }
		}
	}
}
