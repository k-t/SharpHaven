namespace SharpHaven.Game
{
	public interface IMessagePublisher
	{
		void Publish<TMessage>(TMessage message);
	}
}
