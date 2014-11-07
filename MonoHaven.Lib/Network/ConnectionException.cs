using System;

namespace MonoHaven.Network
{
	public class ConnectionException : Exception
	{
		private readonly ConnectionError error;

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
