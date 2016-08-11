using Haven.Messaging;

namespace Haven.Net
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

		public IProtocolHandler Create(IMessagePublisher messagePublisher)
		{
			return new LegacyProtocolHandler(address, messagePublisher);
		}
	}
}
