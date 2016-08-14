using Haven.Net;

namespace Haven.Legacy
{
	public class LegacyProtocolHandlerFactory : IProtocolHandlerFactory
	{
		public IProtocolHandler Create()
		{
			return new LegacyProtocolHandler();
		}
	}
}
