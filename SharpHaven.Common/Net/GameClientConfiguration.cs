using System;

namespace SharpHaven.Net
{
	public class GameClientConfiguration
	{
		public GameClientConfiguration()
		{
		}

		public GameClientConfiguration(NetworkAddress authServerAddress, NetworkAddress gameServerAddress)
		{
			AuthServerAddress = authServerAddress;
			GameServerAddress = gameServerAddress;
		}

		public NetworkAddress AuthServerAddress { get; set; }

		public NetworkAddress GameServerAddress { get; set; }
	}
}
