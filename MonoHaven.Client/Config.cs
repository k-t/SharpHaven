namespace MonoHaven
{
	public static class Config
	{
		static Config()
		{
			AuthHost = "moltke.seatribe.se";
			AuthPort = 1871;
			GameHost = "moltke.seatribe.se";
			GamePort = 1870;
		}

		public static string AuthHost { get; private set; }
		public static int AuthPort { get; private set; }
		public static string GameHost { get; private set; }
		public static int GamePort { get; private set; }
	}
}

