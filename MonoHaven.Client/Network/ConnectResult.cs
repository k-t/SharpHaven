namespace MonoHaven.Network
{
	public enum ConnectResult : byte
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
		public static bool IsSuccessful(this ConnectResult value)
		{
			return value == ConnectResult.Ok;
		}

		public static string GetErrorMessage(this ConnectResult value)
		{
			switch (value)
			{
				case ConnectResult.Ok:
					return "";
				case ConnectResult.InvalidToken:
					return "Invalid authentication token";
				case ConnectResult.AlreadyLoggedIn:
					return "Already logged in";
				case ConnectResult.ConnectionFailed:
					return "Could not connect to server";
				case ConnectResult.OldVersion:
					return "This client is too old";
				case ConnectResult.ExpiredToken:
					return "Authentication token expired";
				default:
					return "Connection failed";
			}
		}
	}
}
