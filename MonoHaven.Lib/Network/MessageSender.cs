using System.Collections.Concurrent;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private const int KeepAliveTimeout = 5000;

		// TODO: remove duplication
		private const int MSG_REL = 1;
		private const int MSG_BEAT = 3;

		private readonly GameSocket socket;
		private readonly BlockingCollection<Message> pending;
		private ushort seq;

		public MessageSender(GameSocket socket) : base("Message Sender")
		{
			this.socket = socket;
			this.pending = new BlockingCollection<Message>();
		}

		protected override void OnStart()
		{
			Message message;
			while (!IsCancelled)
			{
				if (pending.TryTake(out message, KeepAliveTimeout))
				{
					var rmsg = new Message(MSG_REL)
						.Uint16(seq)
						.Byte(message.Type)
						.Bytes(message.GetAllBytes());
					socket.SendMessage(rmsg);
					seq++;
				}
				else
					socket.SendMessage(new Message(MSG_BEAT));
			}
		}

		public void QueueMessage(Message message)
		{
			pending.Add(message);
		}
	}
}
