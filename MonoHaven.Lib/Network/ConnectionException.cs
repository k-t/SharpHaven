using System;

namespace MonoHaven.Network
{
	public class ConnectionException : Exception
	{
		private readonly ConnectionErrorCode errorCode;

		public ConnectionException(string message)
			: base(message)
		{
		}

		public ConnectionException(ConnectionErrorCode errorCode, string message)
			: this(message)
		{
			this.errorCode = errorCode;
		}

		public ConnectionErrorCode ErrorCode
		{
			get { return errorCode; }
		}
	}
}
