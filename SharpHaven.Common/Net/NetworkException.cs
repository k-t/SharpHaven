using System;

namespace SharpHaven.Net
{
	public class NetworkException : Exception
	{
		private readonly ConnectionError error;

		public NetworkException(string message, Exception innerException)
			: base(message, innerException)
		{
			this.error = ConnectionError.ConnectionError;
		}

		public NetworkException(ConnectionError error)
			: base(error.GetMessage())
		{
			this.error = error;
		}

		public ConnectionError Error
		{
			get { return error; }
		}
	}
}
