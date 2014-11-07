using System.Net.Sockets;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private readonly GameSocket socket;

		public MessageSender(GameSocket socket) : base("Message Sender")
		{
			this.socket = socket;
		}

		protected override void OnStart()
		{
		}
	}
}
