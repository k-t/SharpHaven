using Haven.Messaging;

namespace Haven.Net
{
	public interface IProtocolHandlerFactory
	{
		IProtocolHandler Create(IMessagePublisher messagePublisher);
	}
}
