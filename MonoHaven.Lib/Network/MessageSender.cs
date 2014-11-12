using System;
using System.Threading;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private const int KeepAliveTimeout = 5000;
		private const int MSG_BEAT = 3;

		private readonly GameSocket socket;

		public MessageSender(GameSocket socket) : base("Message Sender")
		{
			this.socket = socket;
		}

		protected override void OnStart()
		{
			while (!IsCancelled)
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(KeepAliveTimeout));
				socket.SendMessage(new Message(MSG_BEAT));
			}
		}
	}
}
