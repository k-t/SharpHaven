using System.Net.Sockets;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private readonly Socket socket;

		public MessageSender(Socket socket) : base("Message Sender")
		{
			this.socket = socket;
		}

		protected override void Run()
		{
		}
	}
}
