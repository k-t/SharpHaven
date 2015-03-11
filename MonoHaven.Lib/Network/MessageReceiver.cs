using System;
using MonoHaven.Utils;

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
			MessageReader message;
			while (!IsCancelled)
			{
				if (socket.Receive(out message))
					messageHandler(message);
			}
		}
	}
}
