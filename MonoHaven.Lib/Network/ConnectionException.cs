using System;

namespace MonoHaven.Network
{
	public class ConnectionException : Exception
	{
		private readonly ConnectionError error;

		public ConnectionException(string message)
			: base(message)
		{
		}

		public ConnectionException(ConnectionError error, string message)
			: this(message)
		{
			this.error = error;
		}

		public ConnectionError Error
		{
			get { return error; }
		}
	}
}
