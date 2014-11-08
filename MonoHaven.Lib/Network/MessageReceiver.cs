using System;

namespace MonoHaven.Network
{
	internal class MessageReceiver : BackgroundTask
	{
		private readonly Action<MessageReader> messageHandler;
		private readonly GameSocket socket;

		public MessageReceiver(GameSocket socket, Action<MessageReader> handler)
			: base("Message Receiver")
		{
			this.socket = socket;
			this.messageHandler = handler;
		}

		protected override void OnStart()
		{
			while (!IsCancelled)
				messageHandler(socket.ReceiveMessage());
		}
	}
}
