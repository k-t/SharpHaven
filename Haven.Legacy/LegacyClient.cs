using Haven.Net;

namespace Haven.Legacy
{
	public class LegacyClient : GameClient
	{
		public LegacyClient(GameClientConfig config)
			: base(config, new LegacyAuthHandlerFactory(), new LegacyProtocolHandlerFactory())
		{
		}
	}
}
