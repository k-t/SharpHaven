#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Game
{
	public class GameTime
	{
		public bool IsNight
		{
			get { return false; }
		}

		public double DayTime
		{
			get { return 0.6; }
		}

		public double MoonPhase
		{
			get { return 0.8; }
		}
	}
}
