using System;

namespace MonoHaven.Network
{
	public class AuthStatusEventArgs : EventArgs
	{
		private readonly string status;

		public AuthStatusEventArgs(string status)
		{
			this.status = status;
		}

		public string Status
		{
			get { return status; }
		}
	}
}
