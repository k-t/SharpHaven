﻿using System;

namespace Haven.Net
{
	public interface IAuthHandler : IDisposable
	{
		void Connect(NetworkAddress address);
		byte[] GetToken();
		bool TryPassword(string userName, string password, out byte[] cookie);
		bool TryToken(string userName, byte[] token, out byte[] cookie);
	}
}
