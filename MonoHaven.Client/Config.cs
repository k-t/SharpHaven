using System;

namespace MonoHaven
{
	internal class Config
	{
		public Config()
		{
			AuthHost = "moltke.seatribe.se";
			AuthPort = 1871;
			GameHost = "moltke.seatribe.se";
			GamePort = 1870;
		}

		public string AuthHost { get; private set; }
		public int AuthPort { get; private set; } 
		public string GameHost { get; private set; }
		public int GamePort { get; private set; }
	}
}

