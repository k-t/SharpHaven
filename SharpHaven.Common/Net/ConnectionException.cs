using System;

namespace SharpHaven.Net
{
	public class ConnectionException : Exception
	{
		private readonly ConnectionError error;

		public ConnectionException(string message, Exception innerException)
			: base(message, innerException)
		{
			this.error = ConnectionError.ConnectionError;
		}

		public ConnectionException(ConnectionError error)
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
