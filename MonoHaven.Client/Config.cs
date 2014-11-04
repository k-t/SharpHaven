using MonoHaven.Network;

namespace MonoHaven
{
	public static class Config
	{
		static Config()
		{
			LoginSettings = new LoginSettings
			{
				AuthHost = "moltke.seatribe.se",
				AuthPort = 1871,
				GameHost = "moltke.seatribe.se",
				GamePort = 1870,
			};
		}

		public static LoginSettings LoginSettings
		{
			get;
			private set;
		}
	}
}

