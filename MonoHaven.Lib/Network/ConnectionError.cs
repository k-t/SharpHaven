#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Network
{
	public enum ConnectionError : byte
	{
		None = 0,
		InvalidToken = 1,
		AlreadyLoggedIn = 2,
		ConnectionError = 3,
		InvalidProtocolVersion = 4,
		ExpiredToken = 5
	}

	public static class ConnectionErrorExtensions
	{
		public static string GetMessage(this ConnectionError error)
		{
			switch (error)
			{
				case ConnectionError.None:
					return "";
				case ConnectionError.InvalidToken:
					return "Invalid authentication token";
				case ConnectionError.AlreadyLoggedIn:
					return "Already logged in";
				case ConnectionError.ConnectionError:
					return "Could not connect to server";
				case ConnectionError.InvalidProtocolVersion:
					return "This client is too old";
				case ConnectionError.ExpiredToken:
					return "Authentication token expired";
				default:
					return "Connection failed";
			}
		}
	}
}
