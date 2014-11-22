#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Login;

namespace MonoHaven
{
	public class Config
	{
		public Config()
		{
			LoginSettings = new LoginSettings
			{
				AuthHost = "moltke.seatribe.se",
				AuthPort = 1871,
				GameHost = "moltke.seatribe.se",
				GamePort = 1870,
			};
		}

		public LoginSettings LoginSettings
		{
			get;
			private set;
		}
	}
}

