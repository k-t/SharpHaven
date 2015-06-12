using System;

namespace SharpHaven.Net
{
	public class AuthException : Exception
	{
		public AuthException(string message)
			: base(message)
		{
		}

		public AuthException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
