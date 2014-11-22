#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
