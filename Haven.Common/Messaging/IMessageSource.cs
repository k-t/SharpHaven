namespace Haven.Messaging
{
	public interface IMessageSource
	{
		void Subscribe<TMessage>(MessageHandler<TMessage> handler);

		void Unsubscribe<TMessage>(MessageHandler<TMessage> handler);
	}
}
