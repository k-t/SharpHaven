using MonoHaven.Login;
using MonoHaven.Network;

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

