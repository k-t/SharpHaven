using Haven.Net;

namespace Haven.Legacy
{
	public class LegacyAuthHandlerFactory : IAuthHandlerFactory
	{
		private readonly NetworkAddress address;

		public LegacyAuthHandlerFactory(NetworkAddress address)
		{
			this.address = address;
		}

		public LegacyAuthHandlerFactory(string host, int port)
			: this(new NetworkAddress(host, port))
		{
		}

		public IAuthHandler Create()
		{
			return new LegacyAuthHandler(address);
		}
	}
}
