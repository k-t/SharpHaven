namespace Haven.Messaging
{
	public interface IMessagePublisher
	{
		void Publish<TMessage>(TMessage message);
	}
}
