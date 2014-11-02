using System;

namespace MonoHaven.Network
{
	public class LoginStatusEventArgs : EventArgs
	{
		private readonly string status;

		public LoginStatusEventArgs(string status)
		{
			this.status = status;
		}

		public string Status
		{
			get { return status; }
		}
	}
}
