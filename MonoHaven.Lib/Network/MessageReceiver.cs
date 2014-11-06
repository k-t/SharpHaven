using System;
using System.Net.Sockets;

namespace MonoHaven.Network
{
	internal class MessageReceiver : BackgroundTask
	{
		private const int ReceiveTimeout = 10000000; // 1 sec

		private Action<Message> handler;
		private readonly byte[] receiveBuffer;
		private readonly Socket socket;

		public MessageReceiver(Socket socket) : base("Message Receiver")
		{
			this.socket = socket;
			this.receiveBuffer = new byte[socket.ReceiveBufferSize];
			this.handler = _ => {};
		}

		public void SetHandler(Action<Message> handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			this.handler = handler;
		}

		protected override void Run()
		{
			while (!IsCancelled)
			{
				if (!socket.Poll(ReceiveTimeout, SelectMode.SelectRead))
					continue;

				var message = ReceiveMessage();
				handler(message);
			}
		}

		private Message ReceiveMessage()
		{
			int size = socket.Receive(receiveBuffer);
			if (size == 0)
				throw new InvalidOperationException("Socket is closed");
			var type = receiveBuffer[0];
			var blob = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, blob, 0, size - 1);
			return new Message(type, blob);
		}
	}
}
