using System;

namespace MonoHaven.Network
{
	internal class MessageReceiver : BackgroundTask
	{
		private Action<MessageReader> messageHandler;
		private readonly GameSocket socket;

		public MessageReceiver(GameSocket socket) : base("Message Receiver")
		{
			this.socket = socket;
			this.messageHandler = _ => {};
		}

		public void SetHandler(Action<MessageReader> handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			this.messageHandler = handler;
		}

		protected override void OnStart()
		{
			while (!IsCancelled)
				messageHandler(socket.ReceiveMessage());
		}
	}
}
