using System;

namespace MonoHaven.Network
{
	public class AuthProgressEventArgs : EventArgs
	{
		private readonly string statusText;

		public AuthProgressEventArgs(string statusText)
		{
			this.statusText = statusText;
		}

		public string StatusText
		{
			get { return statusText; }
		}
	}
}
