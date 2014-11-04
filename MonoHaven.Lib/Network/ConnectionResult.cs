namespace MonoHaven.Network
{
	public enum ConnectionResult : byte
	{
		Ok = 0,
		InvalidToken = 1,
		AlreadyLoggedIn = 2,
		ConnectionFailed = 3,
		OldVersion = 4,
		ExpiredToken = 5
	}

	public static class ConnectResultExtensions
	{
		public static bool IsSuccessful(this ConnectionResult value)
		{
			return value == ConnectionResult.Ok;
		}

		public static string GetErrorMessage(this ConnectionResult value)
		{
			switch (value)
			{
				case ConnectionResult.Ok:
					return "";
				case ConnectionResult.InvalidToken:
					return "Invalid authentication token";
				case ConnectionResult.AlreadyLoggedIn:
					return "Already logged in";
				case ConnectionResult.ConnectionFailed:
					return "Could not connect to server";
				case ConnectionResult.OldVersion:
					return "This client is too old";
				case ConnectionResult.ExpiredToken:
					return "Authentication token expired";
				default:
					return "Connection failed";
			}
		}
	}
}
