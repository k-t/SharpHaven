using System;

namespace MonoHaven.Network
{
	public class AuthResultEventArgs : EventArgs
	{
		private readonly string error;
		private readonly byte[] cookie;

		public AuthResultEventArgs(string error)
		{
			this.error = error;
		}

		public AuthResultEventArgs(byte[] cookie)
		{
			this.cookie = cookie;
		}

		public byte[] Cookie
		{
			get { return cookie; }
		}

		public string Error
		{
			get { return error; }
		}

		public bool IsSuccessful
		{
			get { return cookie != null; }
		}
	}
}
