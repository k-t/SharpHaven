using MonoHaven.Network;

namespace MonoHaven
{
	public static class Config
	{
		static Config()
		{
			LoginOptions = new LoginOptions
			{
				AuthHost = "moltke.seatribe.se",
				AuthPort = 1871,
				GameHost = "moltke.seatribe.se",
				GamePort = 1870,
			};
		}

		public static LoginOptions LoginOptions
		{
			get;
			private set;
		}
	}
}

