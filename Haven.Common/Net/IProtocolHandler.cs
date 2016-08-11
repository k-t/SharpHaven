namespace Haven.Net
{
	public interface IProtocolHandler
	{
		void Connect(string userName, byte[] cookie);
		void Close();
		void Send<TMessage>(TMessage message);
	}
}
