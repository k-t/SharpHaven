﻿namespace MonoHaven.Network
{
	public enum ConnectionError : byte
	{
		InvalidToken = 1,
		AlreadyLoggedIn = 2,
		ConnectionError = 3,
		InvalidProtocolVersion = 4,
		ExpiredToken = 5
	}
}