#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven.Network
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
