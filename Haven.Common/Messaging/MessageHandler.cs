namespace Haven.Messaging
{
	public delegate void MessageHandler<in T>(T message);
}
