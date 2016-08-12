﻿using Haven.Net;

namespace Haven.Legacy
{
	public class LegacyProtocolHandlerFactory : IProtocolHandlerFactory
	{
		private readonly NetworkAddress address;

		public LegacyProtocolHandlerFactory(NetworkAddress address)
		{
			this.address = address;
		}

		public LegacyProtocolHandlerFactory(string host, int port)
			: this(new NetworkAddress(host, port))
		{
		}

		public IProtocolHandler Create()
		{
			return new LegacyProtocolHandler(address);
		}
	}
}
