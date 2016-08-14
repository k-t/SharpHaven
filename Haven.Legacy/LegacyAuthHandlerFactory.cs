using Haven.Net;

namespace Haven.Legacy
{
	public class LegacyAuthHandlerFactory : IAuthHandlerFactory
	{
		public IAuthHandler Create()
		{
			return new LegacyAuthHandler();
		}
	}
}
